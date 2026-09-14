using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class UserEndpoints
{
    public static async Task GetPortfolioAsync(Player player, BlobService blobService, PlayerResponse response)
    {

        List<GameSummaryDTO> summaries = [];
        foreach (NationIdentity identity in player.NationIdentities ?? [])
        {
            VGame vGame = await VGame.Load(identity.GameName, blobService);
            List<Nation> nations = await vGame.GatherNationsConfirmDatesAsync(blobService);
            int waitingCount = vGame.GetWaitingCount(nations);
            Nation? myNation = null;
            if (identity.NationCode < 0)
            {
                // < 0 => creator or gamemaster that may not be playing a nation;
                // Pass through as null My Nation only if 
                NationIdentity? match = player.NationIdentities?.Find(i => i.GameName == identity.GameName && i.NationCode > 0);
                if (match is not null) continue;
            }
            else myNation = nations.Find(n => n.Identity.NationCode == identity.NationCode);
            summaries.Add(vGame.CreateGameSummaryDTO(myNation, waitingCount, player));
        }
        response.GameSummaries = summaries;
    }
    //}
    // public static async Task GetPortfolioAsync(User user, BlobService blobService, UserResponse response)
    // {
    //     response.ParticipantInfos = await GetParticipantInfosAsync();
    //     response.GamemasterInfos = await GetGameMasterGamesAsync();

    //     async Task<List<ParticipantInfo>> GetParticipantInfosAsync()
    //     {
    //         List<ParticipantInfo> infos = [];
    //         foreach (NationIdentity identity in user.ParticipantGames)
    //         {
    //             try
    //             {
    //                 //      NationStatus? status = null;
    //                 //      Season? season = null;
    //                 Game game = await Game.RetrieveAsync(identity.GameName, blobService);
    //                 Nation nation = await Nation.RetrieveAsync(identity, blobService);
    //                 if (game.GameState == GameState.Activated)
    //                 {
    //                     //        status = await NationStatus.RetrieveAsync(identity, blobService);
    //                     //      season = await Season.RetrieveAsync(identity.GameName, blobService);
    //                 }
    //                 infos.Add(new ParticipantInfo(user, game, nation));
    //             }
    //             catch
    //             {
    //                 response.AddError(Message.FileNotFoundContinue($"Participant info for {identity.GameName}"));
    //                 continue;
    //             }
    //         }
    //         return infos;
    //     }

    //     async Task<List<GamemasterInfo>> GetGameMasterGamesAsync()
    //     {
    //         List<GamemasterInfo> gamemasterInfos = [];
    //         foreach (string gameName in user.GamemasterGames)
    //         {
    //             try
    //             {
    //                 Game game = await Game.RetrieveAsync(gameName, blobService);
    //                 //   Season? season = null;
    //                 int waitingCount = 0;
    //                 List<Nation> nations = await game.GatherNationsAsync(blobService);
    //                 if (game.GameState == GameState.Created)
    //                 {
    //                     //   List<Nation> nations = await game.GatherAllNationsAsync(blobService);
    //                     foreach (Nation nation in nations) if (nation.EntryStatus != EntryState.Accepted) waitingCount++;
    //                 }
    //                 if (game.GameState == GameState.Activated)
    //                 {
    //                     //   season = await Season.RetrieveAsync(game.Name, blobService);
    //                     //   List<NationStatus> statuses = await game.GatherAllNationStatusesAsync(blobService);
    //                     foreach (Nation status in nations) if (status.Orders?.OrdersState != OrdersState.OrdersSubmitted) waitingCount++;
    //                 }
    //                 gamemasterInfos.Add(new GamemasterInfo(game, waitingCount));
    //             }
    //             catch
    //             {
    //                 response.AddError(Message.FileNotFoundContinue($"Gamemaster info for {gameName}"));
    //                 continue;
    //             }
    //         }
    //         return gamemasterInfos;
    //     }
    // }
}

// public partial class GameSummaryDTO
// {
//     // public GameSummaryDTO(Game game, Nation? nation, int waitingCount, User user)
//     // {
//     //     GameName = game.Name;
//     //     IsCreator = game.Creator == user.UserName;
//     //     IsGamemaster = IsCreator || game.Gamemasters.Contains(user.UserName);
//     //     GameState = game.GameState;
//     //     IsDevelopmentGame = game.IsDevelopmentGame;
//     //     WaitingCount = waitingCount;
//     //     GameDate = game.GameDate?.ToString();
//     //     TimedOut = game.OrdersDueDate < DateTime.UtcNow;
//     //     NationSummary = (nation is null) ? null: new(nation);
//     // }
//     // public static async Task<GameInfo> CreateAsync(Game game, Nation? nation, User user, BlobService blobService)
//     // {
//     //     GameInfo gameInfo = new GameInfo()
//     //     {
//     //         GameName = game.Name,
//     //         IsGamemaster = game.Gamemasters.Contains(user.UserName),
//     //         IsCreator = game.Creator == user.UserName,
//     //         GameState = game.GameState,
//     //         GameDate = game.SeasonDate,
//     //         EntryState = nation?.EntryStatus,
//     //         HomeRegion = nation?.HomeRegion,
//     //         PrimaryName = nation?.PrimaryName,
//     //         OrdersState = nation?.Orders?.OrdersState,
//     //     };
//     //     await AddWaitingCount(gameInfo);
//     //     return gameInfo;


//     //     async Task<int?> AddWaitingCount(GameInfo info)
//     //     {
//     //         if (info.IsGamemaster is false && info.IsCreator is false) return null;
//     //         int count = 0;
//     //         foreach (Nation n in await game.GatherNationsAsync(blobService))
//     //         {
//     //             switch (game.GameState)
//     //             {
//     //                 case ClientShared.GameState.Created:
//     //                     if (n.EntryStatus == ClientShared.EntryState.Invited) count++;
//     //                     break;
//     //                 case ClientShared.GameState.Activated:
//     //                     if (n.Orders?.OrdersState == ClientShared.OrdersState.AwaitOrders) count++;
//     //                     break;
//     //             }
//     //         }
//     //         return count;
//     //     }
//     // }
// }
// public partial class NationSummaryDTO
// {
//     [SetsRequiredMembers]
//     public NationSummaryDTO(Nation nation)
//     {
//         EntryState = nation.LineupState;
//         Naming = nation.Naming;
//         HomeDistrict = nation.HomeDistrict;
//         OrdersState = nation.OrdersState;
//         Identity = nation.Identity;
//     }
// }
