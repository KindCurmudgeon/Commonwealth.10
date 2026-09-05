using Commonwealth.Shared.EndpointDTOs;
using Identity.Client.Service;
using IdentityProvider.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;
public static class CommonEndpoint
{
     public static async Task<PlayerDTO?> GetPlayerDTOAsync(ProfileRequest request, IdentityService identityService)
     {
          ProfileResponse? response = await identityService.GetProfileDTOAsync(request);
          if (response?.ProfileDTO is null) return null;
          return new PlayerDTO(response.ProfileDTO!.UserId, response.ProfileDTO.UserName);
     }
}