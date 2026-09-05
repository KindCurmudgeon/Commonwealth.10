using Authorization;
using Data;
using IdentityProvider.EndpointDTOs;
using Utilities;

namespace IdentityProvider.Endpoints;

public static partial class SigninEndpoints
{
    public static async Task GetToken(AuthProfileDTO profileDTO, BlobService blobService, IConfiguration config, AuthenticateResponse response)
    {
        UserManager userManager = await UserManager.Open(blobService);
        UserProfile userProfile = await userManager.GetValidatedProfile(profileDTO.UserName, profileDTO.Password);
        response.Token = Token.GenerateJwtToken(userProfile, config);
    }
}
