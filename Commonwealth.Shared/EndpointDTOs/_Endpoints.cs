using System.Text.Json.Serialization;

namespace Commonwealth.Shared.EndpointDTOs;

public class RequestBase
{
    public string? Token { get; set; }
}
public class ResponseBase()
{
    public Response Response { get; set; } = new();
}
public class Response
{
    public bool Status { get; set; } = true;
    public List<string> Messages { get; set; } = [];
    public string? ExceptionMessage { get; set;}

}

public static class Endpoints
{
    public const string Gamemaster = "/gamemaster";
    public const string Orders = "/orders";
    public const string Nation = "/nation";
    public const string Admin = "/admin";
    public const string User = "/user";
    public const string Signin = "/signin";
}
public static class Sub
{
    //Admin Sub Endpoints
    public const string UpdateParms = "/updateparms";
    public const string ListUsers = "/listUsers";
    public const string RemoveUser = "/removeUser";
}
