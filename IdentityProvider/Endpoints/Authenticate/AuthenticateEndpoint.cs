using System.Diagnostics;
using IdentityProvider.EndpointDTOs;
using Utilities;

namespace IdentityProvider.Endpoints;

public static partial class AuthEndPoints
{
    public static void AuthenticateEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(IdentityEndpoint.Authenticate, async (
            AuthenticateRequest request,
            BlobService blobService,
            IConfiguration config) =>
        {
            AuthenticateResponse response = new();
            try
            {
                Validate();
                switch (request.RequestType)
                {
                    case AuthenticateRequestType.AUTHENTICATE:
                        await GetToken(request.AuthProfileDTO!, blobService, config, response);
                        break;
                    case AuthenticateRequestType.REGISTER:
                        await Register(request.AuthProfileDTO!, blobService, config, response);
                        break;
                    case (AuthenticateRequestType) 99:
                        await GetUnverifiedToken(request.AuthProfileDTO!, blobService, config, response);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Results.Ok(response);
            }
            return Results.Ok(response);
            void Validate()
            {
                AuthProfileDTO? dto = request.AuthProfileDTO;
                if (dto is null || dto.UserName is null || dto.Password is null) throw new Exception("Invalid credentials");
            }
        });
    }
}

