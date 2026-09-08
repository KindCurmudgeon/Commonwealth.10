// using Data;
// using Utilities;

// namespace IdentityProvider.EndpointDTOs;

// public static partial class ProfileEndpoints
// {
//      public static void ProfileEndpoint(this IEndpointRouteBuilder app)
//      {
//           app.MapPost("/", async (
//               ProfileRequest request,
//               BlobService blobService,
//               IConfiguration config) =>
//           {
//                ProfileResponse response = new();
//                try
//                {
//                     if (request.UserName is not null)
//                     {
//                          UserProfile? profile = await UserProfile.RetrieveAsync(request.UserName, blobService);
//                          response.ProfileDTO = profile?.CreateProfileDTO();
//                     }
//                }
//                catch { response.ProfileDTO = null; }
//                return Results.Ok(response);
//           });
//      }
// }
