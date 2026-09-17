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
        PlayerProfile requestor,
        bool? useHistory,
        BlobService blobService,
        ResponseBase response)
    {
        GameSetup gameSetup = await GameSetup.RetrieveAsync(gameName, blobService);
        gameSetup.ConfirmGamemasterAuthority(requestor);
        GameStatus gameStatus = await GameStatus.RetrieveAsync(gameName, blobService);
        List<Nation> nations;
        if (useHistory is true && gameStatus?.GameDate?.SeasonCount >= 1)
        {
            History history = await History.RetrieveAsync(gameName, gameStatus.GameDate.SeasonCount - 1, blobService);
            gameStatus = history.GameStatus;
            nations = history.Nations;
        }
        else
        {
            nations = await gameStatus!.GatherNationsConfirmDatesAsync(blobService);
            await SaveHistory();
        }

        WorldSetup worldSetup = await WorldSetup.RetrieveAsync(gameName, blobService);
        WorldStatus worldStatus = await WorldStatus.RetrieveAsync(gameName, blobService);
        ServerEconomicMgr econUpdater = new(gameSetup, gameStatus, worldSetup, worldStatus, nations);
        econUpdater.DetermineResults();
        econUpdater.Immigration();
        econUpdater.UpdateForNextSeason();

        List<BlobDescriptor> descriptors = [];
        descriptors.Add(gameStatus.BlobDescriptor());
        foreach (Nation nation in nations) descriptors.Add(nation.BlobDescriptor());
        await blobService.SaveGroupAsync(descriptors);
        response.AddMessage($"{gameStatus.GameName} has been updated for {gameStatus.GameDate.ToString()}");

        async Task SaveHistory()
        {
            History history = new History(gameStatus, nations);
            await history.SaveAsync(blobService);
        }
    }
}
