using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Shared.EndpointDTOs;

public class GameAuthority
{
     public List<string> Gamemasters { get; set; } = [];
     public required string Creator { get; set; }

     public bool HasGamemasterAuthority(PlayerProfile player)
     {
          if (Creator == player.UserName) return true;
          if (Gamemasters.Exists(g => g == player.UserName)) return true;
          if (player.RoleLevel >= RoleLevel.Admin) return true;
          return false;
     }
     public void ConfirmGamemasterAuthority(PlayerProfile player)
     {
          if (HasGamemasterAuthority(player) is false)
               throw new AppException(ExceptionType.Auth, AuthFailType.UserNotAuthorized, player.UserName);
     }
     public void ConfirmGameVisibilityAuthority(PlayerProfile player, List<Nation> nations)
     {
          foreach (Nation nation in nations)
          {
               if (nation.PlayerName == player.UserName) return;
          }
          ConfirmGamemasterAuthority(player);
     }

}