using Commonwealth.Server.Data;
using Commonwealth.Shared.EndpointDTOs;
namespace Commonwealth.Server.Endpoints;

public static class GameEndpointFactory
{
    public static async Task<GameDTO> CreateGameDTOAsync(this GameSetup gameSetup)
    {

        return new GameDTO()
        {
            GameName = gameSetup.GameName,
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




}