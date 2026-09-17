using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using Identity.Client.Service;
using IdentityProvider.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class GamemasterEndpoints
{
    public static async Task UpdateAsync(GameDTO gameDTO, List<LineupDTO>? lineupDTOs, PlayerProfile requestor, BlobService blobService,
        IdentityService identityService, ResponseBase response)
    {
        string gameName = gameDTO.GameName!;
  GameSetup gameSetup = await GameSetup.RetrieveAsync(gameName, blobService);
        gameSetup.ConfirmGamemasterAuthority(requestor);
        List<BlobDescriptor> descriptors = [];
        if (await ProcessGameUpdates() is true) {
            descriptors.Add(gameSetup.BlobDescriptor());
        }
        GameStatus? gameStatus = await GameStatus.RetrieveIfExistsAsync(gameName, blobService);
        await HandleAnyAddedNationsAsync(gameSetup, gameStatus, lineupDTOs, descriptors, blobService, identityService, response);
        await HandleAnyRemovedNationsAsync(gameSetup, lineupDTOs, descriptors, blobService, response);
        await HandlePlayerChanges(gameSetup, gameStatus, lineupDTOs, descriptors, blobService);
        bool result = await blobService.SaveGroupAsync(descriptors);
        if (result is true) response.AddMessage($"Game '{gameName}' updated!");
        else response.AddError($"Trouble updating {gameName}!");

        async Task<bool> ProcessGameUpdates()
        {
            if (gameDTO.ArchiveGame is true)
            {
                await GamemasterEndpoints.RemoveGameAsync(gameName, requestor, blobService, response);
                return true;
            }
            if (gameDTO.OrdersPeriod != gameSetup.OrdersPeriod)
            {
                gameSetup.OrdersPeriod = (TimeSpan)gameDTO.OrdersPeriod!;
                return true;
            }
            return false;
        }

        // bool ProcessParmUpdates()
        // {
        //     bool isParmsModified = false;
        //     return isParmsModified;
        // }
    }

}