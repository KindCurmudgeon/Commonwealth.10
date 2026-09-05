
using Authorization;
using Data;
using IdentityProvider.EndpointDTOs;

namespace IdentityProvider.Endpoints;

public static class UserProfileFactory
{
     public static UserProfile CreateUserProfile(AuthProfileDTO profileDTO)
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
     public static IndexEntry CreateIndexEntry(UserProfile userProfile)
     {
          return new IndexEntry()
          {
               UserName = userProfile.UserName,
               Id = Guid.NewGuid()
          };
     }
}