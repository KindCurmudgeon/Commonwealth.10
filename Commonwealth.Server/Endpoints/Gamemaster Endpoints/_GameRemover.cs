using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class GamemasterEndpoints
{
    public static async Task RemoveGameAsync(string gameName, PlayerProfile requestor, BlobService blobService, ResponseBase response)
    {

        Game game = await Game.RetrieveAsync(gameName, blobService);
        if (game.CreatorName != requestor.UserName)
            throw new AppException(ExceptionType.Auth, AuthFailType.UserNotAuthorized, requestor.UserName);
        await Game.Remove(game, blobService);
        response.AddMessage($"Game '{gameName}' removed!");
    }
}
