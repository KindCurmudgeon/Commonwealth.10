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
        Game game = await Game.RetrieveAsync(gameDTO.GameName!, blobService);
        game.ConfirmGamemasterAuthority(requestor);
        List<BlobDescriptor> descriptors = [];
        if (await ProcessGameUpdates() is true) descriptors.Add(game.BlobDescriptor());
        await HandleAnyAddedNationsAsync(game, lineupDTOs, descriptors, blobService, identityService, response);
        await HandleAnyRemovedNationsAsync(game, lineupDTOs, descriptors, blobService, response);
        await HandlePlayerChanges(game, lineupDTOs, descriptors, blobService);
        bool result = await blobService.SaveGroupAsync(descriptors);
        if (result is true) response.AddMessage($"Game '{game.Name}' updated!");
        else response.AddError($"Trouble updating {game.Name}!");

        async Task<bool> ProcessGameUpdates()
        {
            if (gameDTO.ArchiveGame is true)
            {
         //
                await GamemasterEndpoints.RemoveGameAsync(game.Name, requestor, blobService, response);
                return true;
            }
            if (gameDTO.OrdersPeriod != game.OrdersPeriod)
            {
                game.OrdersPeriod = (TimeSpan)gameDTO.OrdersPeriod!;
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