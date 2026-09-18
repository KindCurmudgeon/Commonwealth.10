
namespace Commonwealth.Server.Data;

public partial class Player {
     public void RemoveAllGames( string gameName)
     {
          NationIdentities.RemoveAll(i => i.GameName == gameName);
          //  user.GamemasterGames.RemoveAll(g => g == gameName);
     }
}