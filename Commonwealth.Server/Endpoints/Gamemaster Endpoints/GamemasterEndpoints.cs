using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class GamemasterEndpoints
{
    public static void GamemasterEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            GamemasterRequest request,
            BlobService blobService) =>
        {
            GamemasterResponse response = new();
            try
            {
                ValidateRequest();
                User user = await Authorization.ValidateUserAsync(request, blobService);
                switch (request.RequestType)
                {
                    case GM_RequestType.Create:
                        await CreateAsync(request.GameDTO!, request.LineupDTOs, user, blobService, response);
                        break;
                    case GM_RequestType.Update:
                        await UpdateAsync(request.GameDTO!, request.LineupDTOs, user, blobService, response);
                        break;
                    case GM_RequestType.Activate:
                        await ActivateAsync(request.GameName!, user, blobService, response);
                        break;
                    case GM_RequestType.SeasonUpdate:
                        await SeasonUpdateAsync(request.GameName!, user, request.UseHistory, blobService, response);
                        break;
                    case GM_RequestType.Get:
                        await GetGameResponseAsync(request.GameName!, user, blobService, response);
                        break;
                    case GM_RequestType.Remove:
                        await RemoveGameAsync(request.GameName!, user, blobService, response);
                        break;
                }
            }
            catch (Exception ex) { response.HandleException(ex); }
            return Results.Ok(response);

            void ValidateRequest()
            {
                switch (request.RequestType)
                {
                    case GM_RequestType.Create:
                    case GM_RequestType.Update:
                        if (request.GameDTO is null) throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, "Game DTO is missing");
                        break;
                    case GM_RequestType.Activate:
                    case GM_RequestType.SeasonUpdate:
                    case GM_RequestType.Get:
                    case GM_RequestType.Remove:
                        if (request.GameName is null) throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, "Game Name is missing");
                        break;

                    default: throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, request.RequestType.ToString());
                }
            }
        });
    }
}
