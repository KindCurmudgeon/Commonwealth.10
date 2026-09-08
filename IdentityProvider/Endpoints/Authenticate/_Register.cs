
using Authorization;
using Data;
using IdentityProvider.EndpointDTOs;
using Utilities;

namespace IdentityProvider.Endpoints;

public static partial class AuthEndPoints
{
     public static async Task Register(AuthProfileDTO authProfile, BlobService blobService, IConfiguration config, AuthenticateResponse response)
     {
          string hashedPassword = PasswordCrypto.HashPassword(authProfile.Password, out byte[] salt);
          UserProfile userProfile = new UserProfile(authProfile, hashedPassword, salt);
          await userProfile.SaveAsync(blobService);
          response.Token = Token.GenerateJwtToken(userProfile, config);
     }

}
