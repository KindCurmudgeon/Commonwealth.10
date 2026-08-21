

using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

public static class Startup
{
    public static async Task InitializeServer(this WebApplication app)
    {
        await app.CreateCreatorUser();
    }

    private static async Task CreateCreatorUser(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        BlobService blobService = scope.ServiceProvider.GetRequiredService<BlobService>();
        bool creatorExists = await User.IsExisting("creator", blobService);
        if (creatorExists is false)
        {
            UserDTO dto = new()
            {
                UserName = "creator",
                Password = "123",
                Email = "grimesbiz@gmail.com",
                FamilyName = "Grimes",
                GivenName = "Michael"
            };
            User creator = new User(dto)
            {
                IsAdministrator = true,
                IsDeveloper = true
            };
            await creator.SaveAsync(blobService);
        }
    }
}