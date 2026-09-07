
using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using IdentityProvider.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class UserEndpoints
{
    public static async Task CreateNewPlayerAsync(ProfileDTO profile, BlobService blobService, PlayerResponse response)
     {
          Player player = new Player()
          {
              UserName = profile.UserName
          };
          await player.SaveAsync(blobService);
     }
}