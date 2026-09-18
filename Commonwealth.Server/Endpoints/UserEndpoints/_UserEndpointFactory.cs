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
     public static Nation GetNation(this List<Nation> nations, NationIdentity identity)
     {
          return nations.First(n => n.Identity.NationCode == identity.NationCode);
     }
         public static int GetProperWaitingCount(this List<Nation> nations, GameState gameState)
    {
        if (gameState == GameState.Activated) return nations.OrdersWaitingCount();
        return nations.AccepetedWaitingCount();
    }
    public static int AccepetedWaitingCount(this List<Nation> nations)
    {
        if (nations.Count == 0) return 0;
        int count = 0;
        foreach (Nation nation in nations)
        {
            if (nation.LineupState != LineupState.Accepted) count++;
        }
        return count;
    }
    public static int OrdersWaitingCount(this List<Nation> nations)
    {
        if (nations.Count == 0) return 0;
        int count = 0;
        foreach (Nation nation in nations)
        {
            if (nation.OrdersState != OrdersState.OrdersSubmitted) count++;
        }
        return count;
    }

}