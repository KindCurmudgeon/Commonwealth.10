using System.Security.Cryptography;
using Authorization;
using Data;
using IdentityProvider.EndpointDTOs;
using Utilities;

namespace IdentityProvider.Endpoints;

public static partial class AuthEndPoints
{
    public static async Task GetToken(AuthProfileDTO authProfile, BlobService blobService, IConfiguration config, AuthenticateResponse response)
    {
        UserProfile? profile = await UserProfile.RetrieveAsync(authProfile.UserName, blobService);
        bool isGood = PasswordCrypto.VerifyPassword(authProfile.Password, profile.HashedPassword, profile.Salt); 
        if (isGood is false) throw new Exception("Invalid Password");
        response.Token = Token.GenerateJwtToken(profile, config);
    }
}
