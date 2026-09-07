using System.Diagnostics.CodeAnalysis;
using Data;

namespace IdentityProvider.EndpointDTOs;

public static class ProfileFactory
{
     public static ProfileDTO CreateProfileDTO(this UserProfile userProfile)
     {
          return new ProfileDTO()
          {
               UserName = userProfile.UserName,
               Email = userProfile.Email,
               GivenName = userProfile.GivenName,
               FamilyName = userProfile.FamilyName,
               IsAdministrator = userProfile.IsAdministrator,
               IsDeveloper = userProfile.IsDeveloper
          };
     }

}