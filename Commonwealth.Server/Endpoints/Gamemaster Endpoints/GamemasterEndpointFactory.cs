using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using Identity.Client.Service;
namespace Commonwealth.Server.Endpoints;

public static class GameEndpointFactory
{
    public static async Task<GameDTO> CreateGameDTOAsync(this GameSetup gameSetup)
    {

        return new GameDTO()
        {
            GameName = gameSetup.GameName,
            EconFileInfo = gameSetup.EconFileInfo,
            GeogFileInfo = gameSetup.GeogFileInfo,
            OrdersPeriod = gameSetup.OrdersPeriod,
            GameState = gameSetup.GameState,
            Creator = gameSetup.Creator,
            Gamemasters = gameSetup.Gamemasters,
            CreationDateTime = DateTime.UtcNow
        };

        // async Task<List<str>?> GetGamemasterDTOs()
        // {
        //     List<string> notFound = [];
        //     List<PlayerDTO> playerDTOs = [];
        //     foreach (string playerName in game.Gamemasters)
        //     {
        //         ProfileRequest request = new() { UserName = playerName };
        //         PlayerDTO? playerDTO = await CommonEndpoint.GetPlayerDTOAsync(request, identityService);
        //         if (playerDTO is null) { notFound.Add(playerName); continue; }
        //         playerDTOs.Add(playerDTO);
        //     }
        //     return playerDTOs;

        // }
    }

    public static LineupDTO CreateLineupDTO(this Nation nation, string player)
    {
        return new LineupDTO()
        {
            Identity = nation.Identity,
            PlayerName = player,
            NationName = nation.Naming?.Name,
            HomeDistrict = nation.HomeDistrict,
            LineupState = nation.LineupState,
            OrdersState = nation.OrdersState
        };
    }
    // public static PlayerDTO CreatePlayerDTO(this ProfileDTO profile)
    // {
    //     return new PlayerDTO(profile.UserId, profile.UserName);
    // }


}