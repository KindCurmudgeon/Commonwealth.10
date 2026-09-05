
using Data;
using IdentityProvider.EndpointDTOs;
using Utilities;

namespace IdentityProvider.Endpoints;

public static partial class SigninEndpoints
{
     public static async Task Register(AuthProfileDTO profileDTO, BlobService blobService, AuthenticateResponse response)
     {

          UserManager userManager = await UserManager.Open(blobService);
          await userManager.AddProfileAsync(profileDTO);
     }

}
