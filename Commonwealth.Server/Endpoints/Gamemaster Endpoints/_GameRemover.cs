using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class GamemasterEndpoints
{
    public static async Task RemoveGameAsync(string gameName, string requestor, BlobService blobService, ResponseBase response)
    {

        Game game = await Game.RetrieveAsync(gameName, blobService);
        if (game.CreatorName != requestor)
            throw new AppException(ExceptionType.Auth, AuthFailType.UserNotAuthorized, requestor);
        await Game.Remove(game, blobService);
        response.AddMessage($"Game '{gameName}' removed!");
        // List<Nation> nations = await game.GatherNationsAsync(blobService);

        // List<BlobDescriptor> descriptors = [];
        // foreach (Nation nation in nations)
        // {
        //     try {
        //     User player = await descriptors.RetrieveIfNotFoundAsync<User>(User.BlobPath(nation.UserName), blobService);
        //     player.RemoveAllGames(game.Name);
        //     }
        //     catch {}
        // }
        // foreach (string userName in game.Gamemasters)
        // {
        //     try
        //     {
        //         User player = await descriptors.RetrieveIfNotFoundAsync<User>(User.BlobPath(userName), blobService);
        //         player.RemoveAllGames(game.Name);
        //     }
        //     catch { }
        // }
        // try
        // {
        //     User creator = await descriptors.RetrieveIfNotFoundAsync<User>(User.BlobPath(game.Creator!), blobService);
        //     creator.RemoveAllGames(game.Name);
        // }
        // catch { }
        // await blobService.SaveGroupAsync(descriptors);
        // await blobService.RemoveWithPrefix(Folders.Games, null, game.Name);


    }
}
