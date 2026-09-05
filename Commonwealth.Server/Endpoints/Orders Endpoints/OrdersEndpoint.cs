
using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class OrdersEndpoints
{
    public static void OrdersEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            OrdersRequest request,
            BlobService blobService,
            ILogger<Program> logger) =>
        {
            OrdersResponse response = new();
            try
            {
                ValidateRequest();
                PlayerDTO requestor = Authorization.ExtractProfileDTOfromToken(request.Token).CreatePlayerDTO();
                Nation nation = await Nation.RetrieveAsync(request.Identity!, blobService);
                if (nation.IsUserAllowed(requestor.Id) is false) throw new AppException(ExceptionType.Auth, AuthFailType.UserNotAuthorized, requestor.UserName);
                //           User player = (requestor.UserName == nation.UserName) ? requestor :  await User.RetrieveAsync(nation.UserName, blobService);


                switch (request.RequestType)
                {
                    case OrdersRequestType.GET:
                        await GetOrdersAsync(nation, blobService, response);
                        break;
                    case OrdersRequestType.UPDATE:
                        logger.LogInformation("Orders Update Request from {username} for {gamename}:{nationname}",
                            requestor.UserName, nation.Identity.GameName, nation.Identity.NationCode);
                        await UpdateOrdersAsync(request, nation, blobService, response);
                        break;
                    default:
                        logger.LogInformation("Unknown Orders Request Type from {username}", requestor.UserName);
                        break;
                }
            }
            catch (Exception ex) { response.HandleException(ex); }
            return Results.Ok(response);

            void ValidateRequest()
            {
                if (request.Identity == null) throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, "Nation Identity");
                switch (request.RequestType)
                {
                    case OrdersRequestType.GET:
                        break;
                    case OrdersRequestType.UPDATE:
                        if (request.SeasonCount is null) throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, "Orders/Date");
                        break;
                    default: throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, request.RequestType.ToString());
                }
            }
        });
    }
}