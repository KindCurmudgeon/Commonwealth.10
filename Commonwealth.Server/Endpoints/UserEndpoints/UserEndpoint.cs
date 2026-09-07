

using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using IdentityProvider.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class UserEndpoints
{
    public static void UserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            PlayerRequest request,
            BlobService blobService) =>
        {
            PlayerResponse response = new();
            try
            {
                Validate();
                ProfileDTO profile = Authorization.ExtractProfileDTOfromToken(request.Token);

                switch (request.RequestType)
                {
                    case PlayerRequestType.NewPlayer:
                        await CreateNewPlayerAsync(profile, blobService, response);
                        break;
                    case PlayerRequestType.GetPortfolio:
                        Player player = await Player.RetrieveAsync(profile.UserName, blobService);
                        await GetPortfolioAsync(player, blobService, response);
                        break;
                        //         case UserRequestType.UPDATE:
                        //             await Update(request.UserDTO!, user, blobService, response);
                        //             break;
                        //         case UserRequestType.GETUSERDTO:
                        //             await GetUserDTO(request.UserName!, blobService, response);
                        //             break;
                        //         case UserRequestType.REMOVE:
                        //             throw new AppException(ExceptionType.Endpoint, EndpointFailType.NotImplemented, request.RequestType.ToString());
                        //         case UserRequestType.CONFIRM:
                        //             await Confirm(request.UserName!, blobService, response);
                        //             break;
                }
                response.PlayerName = profile.UserName;
            }
            catch (Exception ex) { response.HandleException(ex); }
            return Results.Ok(response);

            void Validate()
            {
                if (request.Token is null) throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, "Token");
                // switch (request.RequestType)
                // {
                //     case UserRequestType.UPDATE:
                //         if (request.UserDTO is null) throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, "Updated Account Information");
                //         break;
                //     case UserRequestType.GET_PORTFOLIO:
                //         break;
                //     case UserRequestType.GETUSERDTO:
                //     case UserRequestType.CONFIRM:
                //         if (request.UserName is null) throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, "Name");
                //         break;
                //     default: throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, request.RequestType.ToString());
                // }
            }
        });
    }
}
