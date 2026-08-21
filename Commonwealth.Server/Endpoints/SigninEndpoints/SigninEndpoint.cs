

using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class SigninEndpoints
{
    public static void SigninEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            SigninRequest request,
            BlobService blobService) =>
        {
            SigninResponse response = new();
            try
            {
                Validate();
                switch (request.RequestType)
                {
                    case SigninRequestType.SIGNIN:
                        await SignIn(request.SigninParameters!, blobService, response);
                        break;
                    case SigninRequestType.REGISTER:
                        await Register(request.UserDTO!, blobService, response);
                        break;
                }
            }
            catch (Exception ex) { response.HandleException(ex); }
            return Results.Ok(response);

            void Validate()
            {
                switch (request.RequestType)
                {
                    case SigninRequestType.REGISTER:
                        if (request.UserDTO is null) throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, "Registration"); break;
                    case SigninRequestType.SIGNIN:
                        if (request.SigninParameters is null) throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, "Credentials");
                        break;
                    default: throw new AppException(ExceptionType.Endpoint, EndpointFailType.MissingData, request.RequestType.ToString());
                }
            }
        });
    }
}
