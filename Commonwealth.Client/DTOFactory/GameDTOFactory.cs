using Commonwealth.Shared.EndpointDTOs;


public static class GameDTOFactory
{
     public static GameDTO Create(string creator)
     {
                  return new GameDTO()
        {
            GameName = null,
            GameState = GameState.None,
            CreationDateTime = DateTime.UtcNow,
            Creator = creator,
            OrdersPeriod = new(7, 0, 0, 0),
            Gamemasters = [],
            IsActivated = false
        };
     }
}