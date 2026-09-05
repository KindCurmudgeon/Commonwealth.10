

using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

public static class Startup
{
    public static async Task InitializeServer(this WebApplication app)
    {
        await app.InitializeCreatorPlayer();
   //     await app.CreateCreatorUser();
    }

private static async Task InitializeCreatorPlayer(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        BlobService blobService = scope.ServiceProvider.GetRequiredService<BlobService>();
        bool creatorExists = await Player.IsExisting(Commonwealth.Server.Data.Player.CreatorId, blobService);
        if (creatorExists is false)
        {
            Player player = new Player()
            {
                Id = Commonwealth.Server.Data.Player.CreatorId,
                UserName = "creator",
                NationIdentities = [],
                CreatedDate = DateTime.UtcNow
            };
            await player.SaveAsync(blobService);
        }
    }
    // private static async Task CreateCreatorUser(this WebApplication app)
    // {
    //     using var scope = app.Services.CreateScope();
    //     BlobService blobService = scope.ServiceProvider.GetRequiredService<BlobService>();
    //     bool creatorExists = await Commonwealth.Server.Data.UserIdentity.IsExisting("creator", blobService);
    //     if (creatorExists is false)
    //     {
    //         UserDTO dto = new()
    //         {
    //             UserName = "creator",
    //             Password = "123",
    //             Email = "grimesbiz@gmail.com",
    //             FamilyName = "Grimes",
    //             GivenName = "Michael"
    //         };
    //         Commonwealth.Server.Data.UserIdentity creator = new Commonwealth.Server.Data.UserIdentity(dto)
    //         {
    //             IsAdministrator = true,
    //             IsDeveloper = true
    //         };
    //         await creator.SaveAsync(blobService);
    //     }
    // }
}