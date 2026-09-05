using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using Identity.Client.Service;
using IdentityProvider.EndpointDTOs;
namespace Commonwealth.Server.Endpoints;

public static class GameEndpointFactory
{
    public static async Task<GameDTO> CreateGameDTOAsync(this Game game, IdentityService identityService)
    {

        return new GameDTO()
        {
            GameName = game.Name,
            EconFileInfo = game.EconFileInfo,
            GeogFileInfo = game.GeogFileInfo,
            OrdersPeriod = game.OrdersPeriod,
            GameState = game.GameState,
            Creator = game.Creator,
            Gamemasters = await GetGamemasterDTOs(),
            CreationDateTime = DateTime.UtcNow
        };

        async Task<List<PlayerDTO>?> GetGamemasterDTOs()
        {
            List<Guid> notFound = [];
            List<PlayerDTO> playerDTOs = [];
            foreach (Guid guid in game.Gamemasters)
            {
                ProfileRequest request = new() { UserId = guid };
                PlayerDTO? playerDTO = await CommonEndpoint.GetPlayerDTOAsync(request, identityService);
                if (playerDTO is null) { notFound.Add(guid); continue; }
                playerDTOs.Add(playerDTO);
            }
            return playerDTOs;

        }
    }

    public static LineupDTO CreateLineupDTO(this Nation nation, PlayerDTO player)
    {
        return new LineupDTO()
        {
            Identity = nation.Identity,
            Player = player,
            NationName = nation.Naming?.Name,
            HomeDistrict = nation.HomeDistrict,
            LineupState = nation.LineupState,
            OrdersState = nation.OrdersState
        };
    }
    public static PlayerDTO CreatePlayerDTO(this ProfileDTO profile)
    {
        return new PlayerDTO(profile.UserId, profile.UserName);
    }

}