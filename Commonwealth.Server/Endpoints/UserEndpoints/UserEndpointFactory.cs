using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints;
using Commonwealth.Shared.EndpointDTOs;

public static class UserEndpointFactory
{
     // public static UserDTO CreateDTO(this Commonwealth.Server.Data.UserIdentity user)
     // {
     //      return new UserDTO()
     //      {
     //           UserName = user.UserName,
     //           Password = string.Empty,
     //           Email = user.Email ?? string.Empty,
     //           FamilyName = user.FamilyName ?? string.Empty,
     //           GivenName = user.GivenName ?? string.Empty,
     //           Friends = user.Friends
     //      };
     // }
     public static GameSummaryDTO CreateGameSummaryDTO(GameSetup gameSetup, GameStatus? gameStatus, Nation? nation, string playerName, int waitingCount)
     {
          bool isCreator = gameSetup.Creator == playerName;
          return new GameSummaryDTO()
          {
               GameName = gameSetup.GameName,
               IsCreator = isCreator,
               IsGamemaster = isCreator || gameSetup.Gamemasters.Contains(playerName),
               GameState = gameSetup.GameState,
               WaitingCount = waitingCount,
               GameDate = gameStatus?.GameDate?.ToString(),
               TimedOut = gameStatus?.OrdersDueDate < DateTime.UtcNow,
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