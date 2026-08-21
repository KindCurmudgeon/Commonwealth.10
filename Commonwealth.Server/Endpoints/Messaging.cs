

using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static class Message
{

    // ==========================================================================
    // Non-Exception Messaging
    public static string FileNotFoundContinue(string path) { return $"Trouble reading {path}."; }

    // ===========================================================================
    // Generic Exception Messaging
    public static string Unexpected(string? str = null) { return (str is null) ? "Unexpected Error" : $"Unexpected Error with {str}."; }

    //==========================================================================
    // Authorization Messaging
    public static string InvalidToken() { return "Invalid Authorization"; }
    public static string UnauthorizedUser(string userid) { return $"Unauthorized User: '{userid}'"; }
    public static string ExpiredToken() { return "Authorization has expired. Please Signin again."; }

    //==========================================================================
    // Blob Messaging
    public static string NotFound(string? name, string? aType = null)
    {
        name ??= "<empty>";
        if (aType is not null) aType += " ";
        return $"{aType} '{name}' not found!";
    }
    public static string FileContent(string? str) { return $"Missing required data in {str ?? "unknown"}."; }

    //==========================================================================
    // Endpoint Messaging
    public static string MissingData(string? item) { return $"Missing Information: {item ?? "unknown"}"; }
    public static string NameExists(string? item) { return $"{item}' already exists!"; }
    public static string SeasonUpdated() { return "Season has already been updated!"; }
    public static string FailedUpload(string? item) { return $"Upload of '{item}' Failed!"; }
    public static string NotImplemented(string item) { return $"{item} not implemented"; }
    public static string Invalid(string item) {return $"Invalid {item}";}
}

public static class ResponseExtenstions
{
    public static void AddError(this ResponseBase response, string? message)
    {
        if (message != null) response.Response.Messages.Add(message);
        response.Response.Status = false;
    }
    public static void AddMessage(this ResponseBase response, string? message)
    {
        if (message != null) response.Response.Messages.Add(message);
    }
    public static void AddDetails(this ResponseBase response, string details)
    {
        response.Response.ExceptionMessage = details;
    }
}





// public class LogException : Exception
// {
//     public string UserMessage { get; }
//     public LogException(string message, Exception nativeException) : base(nativeException?.Message, nativeException) {UserMessage = message; }
// }



// public static ResponseBase NullRequest(this ResponseBase response, string? item = null)
// {
//     response.AddError(Message.NULLPARM(item));
//     return response;
// }

// public static T? AddErrorReturnNull<T>(this ResponseBase response, string? message)
// {
//     if (message != null) response.Response.Messages.Add(message);
//     response.Response.Status = false;
//     return default(T);
// }

// public static bool AddErrorReturnFalse(this ResponseBase response, string? message)
// {
//     if (message != null) response.Response.Messages.Add(message);
//     response.Response.Status = false;
//     return false;
// }
// public static Task AddErrorReturnTask(this ResponseBase response, string? message)
// {
//     if (message != null) response.Response.Messages.Add(message);
//     response.Response.Status = false;
// }
