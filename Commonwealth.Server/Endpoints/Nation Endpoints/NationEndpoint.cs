using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using IdentityProvider.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class NationEndpoints
{
    public static void NationEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            NationRequest request,
            BlobService blobService) =>
        {
            NationResponse response = new();
            try
            {
                ValidateRequest();
                PlayerProfile profile = Authorization.ExtractPlayerProfilefromToken(request.Token);
                Nation nation = await Nation.RetrieveAsync(request.Identity, blobService);
                nation.ConfirmPlayerAllowed(profile);
                switch (request.RequestType)
                {
                    case NationRequestType.Get:
                        VGame vGame = await VGame.Load(nation.Identity.GameName, blobService);
                        await ProcessGet(nation, vGame, blobService, response);
                        break;
                    case NationRequestType.Update:
                    case NationRequestType.AcceptReject:
                        await ProcessUpdate(nation, request, blobService, response);
                        break;
                    case NationRequestType.Resign:
                        throw new AppException(ExceptionType.Endpoint, EndpointFailType.NotImplemented, request.RequestType.ToString());
                }
            }
            catch (Exception ex) { response.HandleException(ex); }
            return Results.Ok(response);

            void ValidateRequest()
            {
                switch (request.RequestType)
                {
                    case NationRequestType.Get:
                    case NationRequestType.Resign:
                    case NationRequestType.Update:
                    case NationRequestType.AcceptReject:
                        break;
                    default: throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, request.RequestType.ToString());
                }
                if (request.Identity == null) throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, "Nation Identity");
            }
        });
    }
}