using Data;
using IdentityProvider.EndpointDTOs;
using IdentityProvider.Endpoints;
using Utilities;

public static class Startup
{
    public static async Task Initialize(this WebApplication app)
    {
    }

//     private static async Task CreateCreatorUser(this WebApplication app)
//     {
//         using var scope = app.Services.CreateScope();
//         BlobService blobService = scope.ServiceProvider.GetRequiredService<BlobService>();
//         bool indexExists = await IndexFile.ExistsAsync(blobService);
//         if (indexExists is false)
//         {
//             IndexFile file = new IndexFile();
//             await file.SaveAsync(blobService);
//         }
//         UserManager manager = await UserManager.Open(blobService);
//         if (manager.UserExists("creator")) return;
//         AuthProfileDTO authProfile = new()
//         {
//             UserName = "creator",
//             Password = "123",
//             Email = "grimesbiz@gmail.com",
//             FamilyName = "Grimes",
//             GivenName = "Michael"
//         };
//         UserProfile userProfile = IdentityProvider.Endpoints.ProfileFactory.CreateUserProfile(authProfile);
//         userProfile.IsAdministrator = true;
//         userProfile.IsDeveloper = true;
//         await manager.AddProfileAsync(authProfile);
//     }
}