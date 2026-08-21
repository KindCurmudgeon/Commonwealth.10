using Commonwealth.Shared.EndpointDTOs;

public static class LineupDTOFactory
{
     public static LineupDTO Create(string userName)
     {
          return new LineupDTO()
          {
               Id = Guid.NewGuid(),
               UserName = userName,
               LineupState = LineupState.Added
          };
     }
}