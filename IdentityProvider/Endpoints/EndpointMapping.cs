using IdentityProvider.EndpointDTOs;

namespace IdentityProvider.Endpoints;

public static class EndpointMapping
{
     public static void MapEndpoints(this IEndpointRouteBuilder app)
     {
          //  app.MapGet("/", ()=>{return Results.Ok("Credentials is Operational");});

          app.AuthenticateEndpoint();
     }
}