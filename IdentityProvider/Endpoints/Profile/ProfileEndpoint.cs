using Data;
using Utilities;

namespace IdentityProvider.EndpointDTOs;

public static partial class ProfileEndpoints
{
     public static void ProfileEndpoint(this IEndpointRouteBuilder app)
     {
          app.MapPost("/", async (
              ProfileRequest request,
              BlobService blobService,
              IConfiguration config) =>
          {
               ProfileResponse response = new();
               try
               {
                    UserManager userManager = await UserManager.Open(blobService);
                    if (request.UserName is not null)
                    {
                         UserProfile? profile = await userManager.GetProfileByUserName(request.UserName);
                         if (profile is not null)
                         {
                              response.ProfileDTO = ProfileFactory.CreateProfileDTO(profile);
                         }
                    }
                    else if (request.UserId is not null)
                    {
                         UserProfile userProfile = await userManager.GetProfileById((Guid)request.UserId);
                         response.ProfileDTO = ProfileFactory.CreateProfileDTO(userProfile);
                    }
               }
               catch { response.ProfileDTO = null; }
               return Results.Ok(response);
          });
     }
}
