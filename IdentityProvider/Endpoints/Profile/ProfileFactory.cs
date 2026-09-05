using System.Diagnostics.CodeAnalysis;
using Data;

namespace IdentityProvider.EndpointDTOs;

public static class ProfileFactory
{
     public static ProfileDTO CreateProfileDTO(UserProfile userProfile)
     {
          return new ProfileDTO()
          {
               UserId = userProfile.Id,
               UserName = userProfile.UserName,
               Email = userProfile.Email,
               GivenName = userProfile.GivenName,
               FamilyName = userProfile.FamilyName
          };
     }

}