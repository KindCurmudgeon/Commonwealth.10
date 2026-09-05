using Commonwealth.Shared.EndpointDTOs;

public static class LineupDTOFactory
{
     public static LineupDTO Create(PlayerDTO dto)
     {
          return new LineupDTO()
          {
               LineupId = Guid.NewGuid(),
               Player = dto,
               LineupState = LineupState.Added
          };
     }
}