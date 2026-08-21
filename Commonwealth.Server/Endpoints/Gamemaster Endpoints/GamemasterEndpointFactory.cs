using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
namespace Commonwealth.Server.Endpoints;

public static class GameEndpointFactory
{
    public static async Task<GameDTO> CreateGameDTOAsync(this Game game, BlobService blobService)
    {
        List<UserIdentity> Gamemasters = [];
        foreach (string userName in game.Gamemasters)
        {
            User gameMaster = await User.RetrieveAsync(userName, blobService);
            Gamemasters.Add(gameMaster.CreateUserIdentity());
        }
        return new GameDTO()
        {
            GameName = game.Name,
            EconFileInfo = game.EconFileInfo,
            GeogFileInfo = game.GeogFileInfo,
            OrdersPeriod = game.OrdersPeriod,
            GameState = game.GameState,
            Creator = game.Creator,
            Gamemasters = Gamemasters,
            CreationDateTime = DateTime.UtcNow
        };
    }

    public static LineupDTO CreateLineupDTO(this Nation nation, User user)
    {
        return new LineupDTO()
        {
            Identity = nation.Identity,
            UserName = user.UserName,
            NationName = nation.Naming?.Name,
            HomeDistrict = nation.HomeDistrict,
            LineupState = nation.LineupState,
            OrdersState = nation.OrdersState
        };
    }
    public static UserIdentity CreateUserIdentity(this User user)
    {
        return new UserIdentity()
        {
            UserName = user.UserName,
            Email = user.Email ?? "n/a",
            IsAdmin = user.IsAdministrator,
            IsDeveloper = user.IsDeveloper
        };
    }
}