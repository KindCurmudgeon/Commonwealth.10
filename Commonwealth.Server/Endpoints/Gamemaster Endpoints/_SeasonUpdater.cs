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
        VGame vGame = await VGame.Load(gameName, blobService);
        vGame.ConfirmGamemasterAuthority(requestor);
        GameStatus gameStatus = vGame.ExtractGameStatus();
        List<Nation> nations;
        if (useHistory is true && vGame.GameDate?.SeasonCount >= 1)
        {
            History history = await History.RetrieveAsync(gameName, vGame.GameDate.SeasonCount - 1, blobService);
            GameSetup gameSetup = await GameSetup.RetrieveAsync(vGame.GameName, blobService);
            vGame = new VGame(gameSetup, history.GameStatus);
            nations = history.Nations;
        }
        else
        {
            nations = await vGame.GatherNationsConfirmDatesAsync(blobService);
            await SaveHistory();
        }

        ServerEconomicMgr econUpdater = new(vGame, nations);
        econUpdater.DetermineResults();
        econUpdater.Immigration();
        econUpdater.UpdateForNextSeason();

        List<BlobDescriptor> descriptors = [];
        descriptors.Add(gameStatus.BlobDescriptor());
        foreach (Nation nation in nations) descriptors.Add(nation.BlobDescriptor());
        await blobService.SaveGroupAsync(descriptors);
        response.AddMessage($"{vGame.GameName} has been updated for {vGame.GameDate}");

        async Task SaveHistory()
        {
            History history = new History(gameStatus, nations);
            await history.SaveAsync(blobService);
        }
    }
}
