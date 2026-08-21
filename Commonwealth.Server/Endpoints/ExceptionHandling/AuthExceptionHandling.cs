using System.Diagnostics;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints.ExceptionHandling;

public static class AuthExceptionHandling
{
    public static void HandleAuthException(this ResponseBase response, AppException ex)
    {
        string failedItem = ex.FailedItem ?? "unknown";

        switch (ex.FailType)
        {
            case AuthFailType.InvalidToken:
                response.AddError(Message.InvalidToken());
                break;
            case AuthFailType.ExpiredToken:
                response.AddError(Message.ExpiredToken());
                break;
            case AuthFailType.UserNotAuthorized:
                response.AddError(Message.UnauthorizedUser(failedItem));
                break;
            case AuthFailType.Invalid:
                response.AddError(Message.Invalid(failedItem));
                break;
            default:
                response.AddError(Message.Unexpected("Auth Failure"));
                break;
        }
        response.Response.ExceptionMessage = ex.Message;
        Debug.WriteLine(ex.Message);
    }
}

public static class AuthFailType
{
    public const string InvalidToken = "InvalidToken";
    public const string ExpiredToken = "ExpiredToken";
    public const string UserNotAuthorized = "UserNotAuthorized";
    public const string Invalid = "Invalid";

}