using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.ServerEconomics;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class GamemasterEndpoints
{
    public static async Task SeasonUpdateAsync(
        string gameName,
        string requestor,
        bool? useHistory,
        BlobService blobService,
        ResponseBase response)
    {
        Game game = await Game.RetrieveAsync(gameName, blobService);
        if (game.IsGamemaster(requestor) is false) throw new AppException(ExceptionType.Auth, AuthFailType.UserNotAuthorized, requestor);
        List<Nation> nations;
        if (useHistory is true && game.GameDate?.SeasonCount >= 1)
        {
            History history = await History.RetrieveAsync(gameName, game.GameDate.SeasonCount - 1, blobService);
            game = history.Game;
            nations = history.Nations;
        }
        else
        {
            nations = await game.GatherNationsAsync(blobService);
            await SaveHistory();
        }

        ServerEconomicMgr econUpdater = new(game, nations);
        econUpdater.DetermineResults();
        econUpdater.Immigration();
        econUpdater.UpdateForNextSeason();

        List<BlobDescriptor> descriptors = [];
        descriptors.Add(game.BlobDescriptor());
        foreach (Nation nation in nations) descriptors.Add(nation.BlobDescriptor());
        await blobService.SaveGroupAsync(descriptors);
        response.AddMessage($"{game.Name} has been updated for {game.GameDate}");

        async Task SaveHistory()
        {
            History history = new History(game, nations);
            await history.SaveAsync(blobService);
        }
    }
}
