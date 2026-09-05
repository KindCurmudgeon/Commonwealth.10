using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using Identity.Client.Service;
using IdentityProvider.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class GamemasterEndpoints
{
    public static async Task HandlePlayerChanges(Game game, List<LineupDTO>? lineupDTOs, List<BlobDescriptor> descriptors, BlobService blobService)
    {
        if (lineupDTOs is null) return;
        foreach (LineupDTO dto in lineupDTOs.Where(r => r.OrdersState == OrdersState.Replaced))
        {
            if (dto.NewUser is null || dto.Identity is null) continue;
            // try
            // {
            //     Player newUser = await descriptors.RetrieveIfNotFoundAsync<Data.UserIdentity>(Data.UserIdentity.BlobPath(dto.NewUser), blobService);
            //     Nation nation = await descriptors.RetrieveIfNotFoundAsync<Nation>(Nation.BlobPath(dto.Identity!), blobService);  //new NationIdentity { GameName = dto.Identity!.GameName, NationCode = nationCode }, blobService);
            //     Data.UserIdentity oldUser = await descriptors.RetrieveIfNotFoundAsync<Data.UserIdentity>(Data.UserIdentity.BlobPath(nation.UserName), blobService);

            //     // GameRole gameRole = new GameRole(dto.Identity!.GameName, GameRoleType.Nation, dto.Identity.NationCode);
            //     oldUser.NationIdentities.RemoveAll(i => i.IsSameAs(dto.Identity));
            //     newUser.NationIdentities.Add(dto.Identity);
            //     nation.UserName = newUser.UserName;
            //     nation.LineupState = LineupState.Invited;
            //     game.GameNews?.AddTextEntry($"Player for '{nation.Naming}' changed from '{oldUser.UserName}' to '{newUser.UserName}'.");
            // }
            // catch { continue; }
            // LineupItem? item = roster.Lineup.Find(l => l.NationIdentity == dto.NationIdentity);
            // if (item is null) continue;
            // item.PlayerId = (Guid)dto.PlayerIdentity?.Id!;
            // fileUpdates.Roster = roster;
            // hasChanges = true;
        }
    }

    public static async Task HandleAnyAddedNationsAsync(
        Game game,
        List<LineupDTO>? lineupDTOs,
        List<BlobDescriptor> descriptors,
        BlobService blobService,
        IdentityService IdentityService,
        ResponseBase response)
    {
        List<int> activeCodes = await GatherActiveCodes();
        // if (lineupDTOs?.Count==0)
        // {
        //     LineupDTO dto = new LineupDTO(game.Creator!, new NationIdentity(game.Name,-1));
        //   //  dto.Naming?.SetHomeNaming("Caelnor", game.Districts);
        //     lineupDTOs.Add(dto); 
        // }
        foreach (LineupDTO lineupDTO in lineupDTOs?.Where(r => r.LineupState == LineupState.Added) ?? [])
        {
            List<string> notFound = [];
            ProfileRequest request = new() { UserName = lineupDTO.Player.UserName };
            PlayerDTO? playerDTO = await CommonEndpoint.GetPlayerDTOAsync(request, IdentityService);
            if (playerDTO is null)
            {
                notFound.Add(lineupDTO.Player.UserName);
                continue;
            }

            try
            {
                int code = FindUnusedNationCode();
                Nation nation = new Nation(game, code, playerDTO!.Id);
                if (lineupDTO.HomeDistrict is not null) nation.HomeDistrict = lineupDTO.HomeDistrict;
                Player player = await descriptors.AddIfNotDuplicateAsync<Player>(Player.BlobPath(lineupDTO.Player.Id), blobService);
                player?.NationIdentities.Add(nation.Identity);
                descriptors.Add(nation.BlobDescriptor());
                if (game.GameState == GameState.Activated)
                {
                    game.WorldNews?.AddTextEntry($"Nation '{nation.Naming?.Name}' mangaged by '{player?.Id}' has joined the game.");
                    descriptors.AddIfNotDuplicate(game.BlobDescriptor());
                }
            }
            catch { response.AddError(Message.NotFound(playerDTO!.UserName, "Player")); continue; }
        }

        async Task<List<int>> GatherActiveCodes()
        {
            List<int> codes = [];
            List<Nation> existingNations = await game.GatherNationsAsync(blobService);
            foreach (Nation nation in existingNations) codes.Add(nation.Identity.NationCode);
            return codes;
        }

        int FindUnusedNationCode()
        {
            int index = 0;
            int found = -1;
            do
            {
                index += 10;
                found = activeCodes.FindIndex(c => c == index);
            } while (found >= 0);
            activeCodes.Add(index);
            return index;
        }


    }
    public static async Task HandleAnyRemovedNationsAsync(Game game, List<LineupDTO>? lineupDTOs, List<BlobDescriptor> descriptors, BlobService blobService, ResponseBase response)
    {
        foreach (LineupDTO lineupDTO in lineupDTOs!.Where(r => r.OrdersState == OrdersState.Remove))
        {
            if (lineupDTO.Identity is null) continue;
            NationIdentity identity = lineupDTO.Identity;
            //    GameRole gameRole = new GameRole(game.Name, GameRoleType.Nation, identity.NationCode);
            if (game.GameState == GameState.Created)
            {
                Player user = await descriptors.AddIfNotDuplicateAsync<Player>(Player.BlobPath(lineupDTO.Player.Id), blobService);
                user?.NationIdentities.RemoveAll(g => g.IsSameAs(identity));
                descriptors.Add(Nation.BlobDescriptorRemove(identity));
                response.AddMessage($"Nation '{identity.GameName}:{identity.NationCode}' removed.");
                return;
            }
            if (game.GameState == GameState.Activated)
            {
                Nation nation = await descriptors.AddIfNotDuplicateAsync<Nation>(Nation.BlobPath(identity), blobService);
                //   nation.LineupState = LineupState.ToBeRemoved;
                nation.OrdersState = OrdersState.Remove;
                response.AddMessage($"Nation '{nation.Naming.Name}' marked for removal.");
            }
        }
    }
}

