
using Authorization;
using Data;
using IdentityProvider.EndpointDTOs;
using Utilities;

namespace IdentityProvider.Endpoints;

public static partial class AuthEndPoints
{
     public static async Task Register(AuthProfileDTO authProfile, BlobService blobService, IConfiguration config, AuthenticateResponse response)
     {
          UserProfile userProfile = CreateUserProfile(authProfile);
          await userProfile.SaveAsync(blobService);
          response.Token = Token.GenerateJwtToken(userProfile, config);
     }
     private static UserProfile CreateUserProfile(AuthProfileDTO profileDTO)
     {
          string hashedPassword = PasswordCrypto.HashPassword(profileDTO.Password, out byte[] salt);
          return new UserProfile()
          {
               UserName = profileDTO.UserName,
               HashedPassword = hashedPassword,
               Salt = salt,
               Id = Guid.NewGuid(),
               CreationTime = DateTime.UtcNow,
               Email = profileDTO.Email,
               FamilyName = profileDTO.FamilyName,
               GivenName = profileDTO.GivenName,
               IsAdministrator = false,
               IsDeveloper = false
          };
     }

}
