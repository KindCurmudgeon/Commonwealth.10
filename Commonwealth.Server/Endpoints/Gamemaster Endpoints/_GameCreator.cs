using System.Text.Json;
using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Parameters;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EndpointDTOs;
using Identity.Client.Service;
using IdentityProvider.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class GamemasterEndpoints
{
    public static async Task CreateAsync(GameDTO gameDTO, List<LineupDTO>? lineupDTOs, PlayerProfile creator,
                            BlobService blobService, IdentityService identityService, ResponseBase response)
    {
        bool confirm = Util.IsLegalWindowsFilename(gameDTO.GameName);
        if (confirm is false) throw new AppException(ExceptionType.Endpoint, EndpointFailType.Invalid, "Illegal GameName");
        string? gameName = Util.TitleCase(gameDTO.GameName);
        if (await GameSetup.GameExists(gameName, blobService)) gameName = await FindUnique(gameName, blobService);
        if (gameName is null)
        {
            throw new AppException(ExceptionType.Endpoint, EndpointFailType.NameExists, gameName);
        }

        InitParms initParms = new InitParms((gameDTO.InitFileInfo is null) ? null :
            await InitParmsFile.RetrieveAsync(gameDTO.InitFileInfo, blobService));

        // use EconParmsFile if provided, otherwise load default EconParms    
        EconParms econParms = EconParmsHelpers.LoadEconParms(null);

        GeographyFile geogParmsFile = await GeographyFile.RetrieveAsync(gameDTO.GeogFileInfo, blobService);

        GameSetup gameSetup = GameSetup.Create(gameDTO, creator, econParms, geogParmsFile, initParms);
        Player creatorPlayer = await Player.RetrieveAsync(creator.UserName, blobService);
        creatorPlayer.NationIdentities.Add(new NationIdentity(gameSetup.GameName, -1)); // -1 => Creator
        List<BlobDescriptor> descriptors = [];
        descriptors.Add(gameSetup.BlobDescriptor());
        descriptors.Add(creatorPlayer.BlobDescriptor());

        await HandleAnyAddedNationsAsync(gameSetup, lineupDTOs, descriptors, blobService, identityService, response);
        bool result = await blobService.SaveGroupAsync(descriptors);
        if (result is true) response.AddMessage($"Game '{gameSetup.GameName}' created!");
        else response.AddError($"Trouble creating {gameSetup.GameName}!");
    }

    public static class Defaults
    {
        public static readonly string GeographyFileName = "Test";
        public static readonly string EconFileName = "Test";
        public static readonly string SeasonParmsFileName = "Test";
        public static readonly string InitParmsFileName = "Test";
    }
    public static async Task<string?> FindUnique(string name, BlobService blobService, int? attempt = 0)
    {
        if (++attempt >= 6) return null;
        string? newName = AppendNumber(name);
        if (await GameSetup.GameExists(newName, blobService) is true)
            return await FindUnique(newName, blobService, attempt);
        else return newName;

        static string AppendNumber(string source)
        {
            int n = Util.GetRandomInclusive(0, 9);
            return $"{source}{n}";
        }
    }
}