using System.Diagnostics.CodeAnalysis;
using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using Identity.Client.Service;


namespace Commonwealth.Server.Endpoints;


public static partial class GamemasterEndpoints
{

    public static async Task GetGameResponseAsync(string gameName, string requestor, 
            BlobService blobService, IdentityService identityService, GamemasterResponse response)
    {
        Game game = await Game.RetrieveAsync(gameName, blobService);
        List<Nation> nations = await game.GatherNationsAsync(blobService);
        game.ConfirmUserIsAllowed(requestor, nations);
        //    GameParms gameParms = await GameParms.RetrieveAsync(game.Name, blobService);
        response.GameDTO = await game.CreateGameDTOAsync(identityService);
        response.LineupDTOs = await CreateLineupDTOAsync();
        response.IsGamemaster = game.IsGamemaster(requestor);
        if (response.IsGamemaster is true)
        {
            Player player = await Player.RetrieveAsync(requestor, blobService);
            response.Friends = player.Friends;
        }

        async Task<List<LineupDTO>> CreateLineupDTOAsync()
        {
            List<LineupDTO> dtos = [];
            foreach (Nation nation in nations)
            {
                LineupDTO dto = nation.CreateLineupDTO(nation.PlayerName);
                dtos.Add(dto);
            }
            return dtos;
        }


    }
}

// public partial class GameDTO
// {
//     public static async Task<GameDTO> CreateAsync(Game game, BlobService blobService)
//     {
//         List<UserIdentity> Gamemasters = [];
//         foreach (string userName in game.Gamemasters)
//         {
//             User gameMaster = await User.RetrieveAsync(userName, blobService);
//             //   User gameMaster = await blobService.RetrieveAsync<User>(User.BlobPath(userName));
//             Gamemasters.Add(new UserIdentity(gameMaster));
//         }
//         return new GameDTO()
//         {
//             GameName = game.Name,
//             EconFileInfo = game.EconFileInfo,
//             GeogFileInfo = game.GeogFileInfo,
//             //   SeasonFileInfo = gameParms.SeasonFileInfo,
//             OrdersPeriod = game.OrdersPeriod,
//             GameState = game.GameState,
//             Creator = game.Creator,
//             Gamemasters = Gamemasters,
//             CreationDateTime = DateTime.UtcNow
//         };
//     }
// }
// public partial class LineupDTO
// {
//     [SetsRequiredMembers] 
//     public LineupDTO(Nation nation, User user)
//     {
//         Identity = nation.Identity;
//         UserName = user.UserName;
//         NationName = nation.Naming?.Name;
//         HomeDistrict = nation.HomeDistrict;
//         LineupState = nation.LineupState;
//         OrdersState = nation.OrdersState;
//     }
// }
// public partial class UserIdentity
// {
//     [SetsRequiredMembers]
//     public UserIdentity(User user)
//     {
//         UserName = user.UserName;
//         Email = user.Email ?? "n/a";
//         IsAdmin = user.IsAdministrator;
//         IsDeveloper = user.IsDeveloper;
//     }
// }