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
                ProfileDTO profile = Authorization.ExtractProfileDTOfromToken(request.Token);
                Nation nation = await Nation.RetrieveAsync(request.Identity, blobService);
                if (nation.IsUserAllowed(profile.UserId) is false) throw new AppException(ExceptionType.Auth, AuthFailType.UserNotAuthorized, profile.UserName);
                switch (request.RequestType)
                {
                    case NationRequestType.Get:
                    // Game game = await Game.RetrieveAsync(nation.Identity.GameName, blobService);
                    // //    GameParms gameParms = await GameParms.RetrieveAsync(nation.Identity.GameName, blobService);
                    // Data.UserIdentity player = (requestor.UserName == nation.UserName) ? requestor : await Data.UserIdentity.RetrieveAsync(nation.UserName, blobService);
                    // await ProcessGet(nation, game, blobService, response);
                    // break;
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