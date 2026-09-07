using Commonwealth.Shared.EndpointDTOs;

public static class LineupDTOFactory
{
     public static LineupDTO Create(string playerName)
     {
          return new LineupDTO()
          {
               LineupId = Guid.NewGuid(),
               PlayerName = playerName,
               LineupState = LineupState.Added
          };
     }
}