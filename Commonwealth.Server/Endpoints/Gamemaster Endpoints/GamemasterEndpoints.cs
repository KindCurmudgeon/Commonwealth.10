using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using Identity.Client.Service;
using IdentityProvider.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class GamemasterEndpoints
{
    public static void GamemasterEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            GamemasterRequest request,
            BlobService blobService, IdentityService identityService) =>
        {
            GamemasterResponse response = new();
            try
            {
                ValidateRequest();
                PlayerDTO requestor = Authorization.ExtractProfileDTOfromToken(request.Token).CreatePlayerDTO();
                switch (request.RequestType)
                {
                    case GM_RequestType.Create:
                        await CreateAsync(request.GameDTO!, request.LineupDTOs, requestor, blobService, identityService, response);
                        break;
                    case GM_RequestType.Update:
                        await UpdateAsync(request.GameDTO!, request.LineupDTOs, requestor, blobService, identityService, response);
                        break;
                    case GM_RequestType.Activate:
                        await ActivateAsync(request.GameName!, requestor, blobService, response);
                        break;
                    case GM_RequestType.SeasonUpdate:
                        await SeasonUpdateAsync(request.GameName!, requestor, request.UseHistory, blobService, response);
                        break;
                    case GM_RequestType.Get:
                        await GetGameResponseAsync(request.GameName!, requestor, blobService, identityService, response);
                        break;
                    case GM_RequestType.Remove:
                        await RemoveGameAsync(request.GameName!, requestor, blobService, response);
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
