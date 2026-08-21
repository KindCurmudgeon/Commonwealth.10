using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

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
                User requestor = await Authorization.ValidateUserAsync(request, blobService);
                Nation nation = await Nation.RetrieveAsync(request.Identity!, blobService);
                if (nation.IsUserAllowed(requestor) is false) throw new AppException(ExceptionType.Auth, AuthFailType.UserNotAuthorized, requestor.UserName);
                switch (request.RequestType)
                {
                    case NationRequestType.Get:
                        Game game = await Game.RetrieveAsync(nation.Identity.GameName, blobService);
                        //    GameParms gameParms = await GameParms.RetrieveAsync(nation.Identity.GameName, blobService);
                        User player = (requestor.UserName == nation.UserName) ? requestor : await User.RetrieveAsync(nation.UserName, blobService);
                        await ProcessGet(nation, game, blobService, response);
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