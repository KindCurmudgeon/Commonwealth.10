

using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using IdentityProvider.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static class AdminActionEndpoint
{
    public static void AdminAction(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            AdminRequest request,
            BlobService blobService) =>
        {
            AdminResponse response = new() { Action = request.Action };
            try
            {
                PlayerProfile profile = Authorization.ExtractPlayerProfilefromToken(request.Token);
                // if (requester.IsDeveloper is not true)
                //     throw new AppException(ExceptionType.Auth, AuthFailType.UserNotAuthorized, requester.UserName);
                response.Items = [];
                response.Action = request.Action;
                response.Arg1 = request.Arg1;
                response.Arg2 = request.Arg2;
                switch (request.Action)
                {
                    case Actions.GetList: await GetList(request.Arg1); break;
                    case Actions.RemoveFile: await RemoveFile(request.Arg1); break;
                    case Actions.ViewFile: await ViewFile(request.Arg1); break;
                    case Actions.GetPlayerGames: await GetUserGames(request.Arg1); break;
                    case Actions.GetGamePlayers: await GetGameUsers(request.Arg1); break;
                    case Actions.RemoveGameFromPlayer: await RemoveGameFromUser(request.Arg1); break;
                    // case Actions.MakeAdmin: await MakeAdmin(request.Arg1); break;
                    // case Actions.MakeDev: await MakeDev(request.Arg1); break;
                    case Actions.GetGames: await GetGames(); break;
                    case Actions.RemoveGame: await RemoveGame(request.Arg1); break;
                }
            }
            catch (Exception ex) { response.HandleException(ex); }
            return Results.Ok(response);


            async Task<bool> GetList(string? listType)
            {
                string? prefix = null;
                switch (listType)
                {
                    case FileTypes.Player: prefix = Folders.AllPlayersPrefix(); break;
                    case FileTypes.Game: prefix = Folders.AllGamesBlobPrefix(); break;
                    case FileTypes.Parm: prefix = ParmsBlobPath(); break;
                    case FileTypes.AllFiles: prefix = ""; break;
                    default: return false;
                }
                try
                {
                    response.Items = await blobService.GetFileNamesWithPrefix(prefix);
                }
                catch
                {
                    response.AddError("Error Creating List");
                }
                return true;
            }

            async Task GetUserGames(string? userFileName)
            {
                if (userFileName is null) return;
                Player player = await blobService.RetrieveAsync<Player>(userFileName);
                foreach (NationIdentity identity in player.NationIdentities)
                {
                    response.Items.Add($"{CreateIdentityString(identity)}");
                }
                //    foreach (string gameName in user.GamemasterGames) response.Items.Add(gameName);

                string CreateIdentityString(NationIdentity identity)
                {
                    return $"{identity.GameName},{identity.NationCode}";
                }
            }
            async Task GetGameUsers(string? gameFileName)
            {
                if (gameFileName is null) return;
                string gameName = Path.GetFileNameWithoutExtension(gameFileName);
                VGame vGame = await VGame.Load(gameName, blobService);
                List<Nation> nations = await vGame.GatherNationsConfirmDatesAsync(blobService);
                //     response.Object = await blobService.RetrieveAsync<User>(userFileName);
            }
            async Task RemoveGameFromUser(string? userFileName)
            {
                if (userFileName is null) return;
                Player player = await blobService.RetrieveAsync<Player>(userFileName);
                string[] parsed = request.Arg2!.Split(",").ToArray();
                string gameName = parsed[0];
                player.NationIdentities.RemoveAll(i => i.GameName == gameName);
                await player.SaveAsync(blobService);
            }
            async Task<bool> RemoveFile(string? fileName)
            {
                if (fileName is null) return false;
                await blobService.RemoveJsonAsync(fileName);
                return true;

                // async Task RemoveUser()
                // {
                //     await blobService.RemoveJsonAsync(User.BlobPath(request.Target!));
                // }
                // async Task RemoveGame()
                // {
                //     Game game = await blobService.RetrieveAsync<Game>(Game.BlobPath(request.Target!));
                //     List<Nation> nations = await Game.GatherNationsAsync(game.Name, blobService);
                //     foreach (Nation nation in nations)
                //     {
                //         try
                //         {
                //             User user = await blobService.RetrieveAsync<User>(nation.UserName);
                //             user.RemoveAllGames(request.Target!);
                //         }
                //         catch
                //         {
                //             continue;
                //         }
                //     }
                //     try
                //     {
                //         await blobService.RemoveWithPrefix(Folders.Games, game.Name);
                //     }
                //     catch { throw new Exception($"Error removing Game '{game.Name}"); }                  
                // }
            }
            // async Task RemoveGameFromUser(string? userFileName, string? identifier)
            // {
            //     if (userFileName is null || identifier is null) return;

            //     User user = await blobService.RetrieveAsync<User>(userFileName);
            //     List<string> parsed = identifier.Split(",").ToList();
            //     switch (parsed.Count)
            //     {
            //         case 1: user.GamemasterGames.Remove(parsed[0]); break;
            //         default:
            //             user.ParticipantGames.RemoveAll(i=>i.GameName == parsed[0] && i.NationCode.ToString() == parsed[2]);
            //         break;
            //     }
            //     await user.SaveAsync(blobService);
            // }

            async Task ViewFile(string? fileName)
            {
                if (fileName is null) return;
                response.JsonString = await blobService.GetJsonStringAsync(fileName);
            }
            // async Task<bool> UpdateParms()
            // {
            //     try
            //     {
            //         FileParmsRaw fileParmsRaw = await fileService.ReadJsonDataAsync<FileParmsRaw>(Folders.Parms, null, "GameParmsRaw");
            //         GeographyParmsRaw geographyParmsRaw = await fileService.ReadJsonDataAsync<GeographyParmsRaw>(Folders.Parms, null, "TestWorld");
            //         Naming naming = await fileService.ReadJsonDataAsync<Naming>(Folders.Parms, null, "Naming");
            //         await blobService.SaveAsync(fileParmsRaw.BlobDescriptor());
            //         await blobService.SaveAsync(geographyParmsRaw.BlobDescriptor());
            //         await blobService.SaveAsync(naming.BlobDescriptor());

            //         GeographyParmsRaw geogRaw = await blobService.RetrieveAsync<GeographyParmsRaw>(geographyParmsRaw.BlobPath());

            //         response.AddMessage("Parm Files Updated");
            //     }
            //     catch (Exception ex)
            //     {
            //         response.AddError(ex.Message);
            //     }
            //     return true;
            // }
            // async Task<bool> MakeAdmin(string? fileName)
            // {
            //     if (fileName is null) return false;
            //     Data.UserIdentity user = await blobService.RetrieveAsync<Data.UserIdentity>(fileName);
            //     user.IsAdministrator = true;
            //     await user.SaveAsync(blobService);
            //     return true;
            // }
            // async Task<bool> MakeDev(string? fileName)
            // {
            //     if (fileName is null) return false;
            //     Data.UserIdentity user = await blobService.RetrieveAsync<Data.UserIdentity>(fileName);
            //     user.IsAdministrator = true;
            //     user.IsDeveloper = true;
            //     await user.SaveAsync(blobService);
            //     return true;
            // }
            async Task GetGames()
            {
                string prefix = Folders.AllGamesBlobPrefix();
                List<string> gamefiles = await blobService.GetFileNamesWithPrefix(prefix);
                foreach (string gamefile in gamefiles)
                {
                    List<string> parsed = gamefile.Split('/').ToList();
                    string fileName = Path.GetFileNameWithoutExtension(parsed[1]);
                    parsed = fileName.Split('-').ToList();
                    response.Items.AddIfNotDuplicate(parsed[0]);
                }
            }
            async Task RemoveGame(string? gameName)

            {
                if (gameName is null) return;
                GameSetup gameSetup = await GameSetup.RetrieveAsync(gameName, blobService);
                await GameSetup.Remove(gameSetup, blobService);
                response.AddMessage($"Game '{gameName}' removed!");
            }
        });
    }
    public static string ParmsBlobPath() { return BlobService.CreateBlobPrefix(Folders.Parms, null, null); }
}


