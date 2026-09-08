using Authorization;
using Data;
using IdentityProvider.EndpointDTOs;
using Utilities;

namespace IdentityProvider.Endpoints;

public static partial class AuthEndPoints
{
    public static async Task GetUnverifiedToken(AuthProfileDTO authProfile, BlobService blobService, IConfiguration config, AuthenticateResponse response)
    {
        UserProfile? profile = await UserProfile.RetrieveAsync(authProfile.UserName, blobService);
        response.Token = Token.GenerateJwtToken(profile, config);
    }
}