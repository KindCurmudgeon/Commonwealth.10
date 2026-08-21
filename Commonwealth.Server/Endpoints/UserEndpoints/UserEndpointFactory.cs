using Commonwealth.Server.Data;
using Commonwealth.Shared.EndpointDTOs;

public static class UserEndpointFactory
{
     public static UserDTO CreateDTO(this User user)
     {
          return new UserDTO()
          {
               UserName = user.UserName,
               Password = string.Empty,
               Email = user.Email ?? string.Empty,
               FamilyName = user.FamilyName ?? string.Empty,
               GivenName = user.GivenName ?? string.Empty,
               Friends = user.Friends
          };
     }
     public static GameSummaryDTO CreateGameSummaryDTO(this Game game, Nation? nation, int waitingCount, User user)
     {
          bool isCreator = game.Creator == user.UserName;
          return new GameSummaryDTO()
          {
               GameName = game.Name,
               IsCreator = isCreator,
               IsGamemaster = isCreator || game.Gamemasters.Contains(user.UserName),
               GameState = game.GameState,
               IsDevelopmentGame = game.IsDevelopmentGame,
               WaitingCount = waitingCount,
               GameDate = game.GameDate?.ToString(),
               TimedOut = game.OrdersDueDate < DateTime.UtcNow,
               NationSummary = nation?.CreateNationSummaryDTO()
          };
     }

     public static NationSummaryDTO CreateNationSummaryDTO(this Nation nation)
     {
          return new NationSummaryDTO()
          {
               Identity = nation.Identity,
               EntryState = nation.LineupState,
               Naming = nation.Naming,
               HomeDistrict = nation.HomeDistrict,
               OrdersState = nation.OrdersState
          };
     }
}