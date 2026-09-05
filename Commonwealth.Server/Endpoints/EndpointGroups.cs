using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static class EndpointGroups
{
  public static void MapEndpoints(this IEndpointRouteBuilder app)
  {
    // var signinGroup = app.MapGroup(Commonwealth.Shared.EndpointDTOs.Endpoints.Signin);
    // signinGroup.SigninEndpoint();

    var userGroup = app.MapGroup(Shared.EndpointDTOs.Endpoints.User);
    userGroup.UserEndpoint();
  
    var gamemasterGroup = app.MapGroup(Shared.EndpointDTOs.Endpoints.Gamemaster);
    gamemasterGroup.GamemasterEndpoint();

    var ordersGroup = app.MapGroup(Shared.EndpointDTOs.Endpoints.Orders);
    ordersGroup.OrdersEndpoint();

    var nationGroup = app.MapGroup(Shared.EndpointDTOs.Endpoints.Nation);
    nationGroup.NationEndpoint();

    var adminGroup = app.MapGroup(Shared.EndpointDTOs.Endpoints.Admin);
    adminGroup.AdminAction();
    adminGroup.AdminUpdateParms();
  }
}




