using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class GamemasterEndpoints
{
    public static async Task RemoveGameAsync(string gameName, PlayerProfile requestor, BlobService blobService, ResponseBase response)
    {

        GameSetup gameSetup = await GameSetup.RetrieveAsync(gameName, blobService);
        if (gameSetup.Creator != requestor.UserName && requestor.RoleLevel < RoleLevel.Admin)
            throw new AppException(ExceptionType.Auth, AuthFailType.UserNotAuthorized, requestor.UserName);
        await GameSetup.Remove(gameSetup, blobService);
        response.AddMessage($"Game '{gameName}' removed!");
    }
}
