using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints.ExceptionHandling;

public static class ExceptionMgr
{
    public static void HandleException(this ResponseBase response, Exception ex)
    {
        if (ex is AppException AppEx)
        {
            switch (AppEx.ExceptionType)
            {
                case ExceptionType.Blob:
                    response.HandleBlobException(AppEx);
                    break;
                case ExceptionType.Auth:
                    response.HandleAuthException(AppEx);
                    break;
                case ExceptionType.Endpoint:
                    response.HandleEndpointException(AppEx);
                    break;
                default:
                    response.AddError(Message.Unexpected("Exception Type"));
                    response.Response.ExceptionMessage = ex.Message;
                    break;
            }
        }
        else
        {
            response.AddError(Message.Unexpected("Not AppException"));
            response.Response.ExceptionMessage = ex.Message;
        }

    }

    //     public static void AddExceptionUserMsg(this Exception ex, string? UserMessage = null)
    // {
    //     if (UserMessage is not null) ex.Data[Message.MSGFLAG] = UserMessage;
    // }
    // public static void AddExceptionDetails(this Exception ex, ExceptionType type, ExceptionCode code)
    // {
    //     ex.Data[ExType] = type;
    //     ex.Data[FailType] = code;
    // }
}
public enum ExceptionType { NONE, Blob, Auth, Endpoint }

public class AppException : Exception
{
    public ExceptionType? ExceptionType { get; set; }
    public string? FailType { get; set; }
    public string? FailedItem { get; set; }
    public AppException(ExceptionType exceptionType, string failureType, Exception? innerException = null) : base("Application Exception", innerException)
    {
        ExceptionType = exceptionType;
        FailType = failureType;
    }
    public AppException(ExceptionType exceptionType, string failureType, string? failedItem) : base("Application Exception", null)
    {
        ExceptionType = exceptionType;
        FailType = failureType;
        FailedItem = failedItem;
    }
}
public static class AppExceptionExtensions
{
    public static void AddFailedItem(this Exception ex, string? failedItem)
    {
        if (ex is AppException appEx) appEx.FailedItem = failedItem;
    }
}



public static class BlobFailMessage
{
    public static string NOTFOUND(string item) => $"{item} was not found.";
    public static string JSON(string item) => $"{item} information looks corrupted.";
    public static string TROUBLEWRITE(string item) => $"There was a problem saving {item}.";
    public static string UNEXPECTED(string item) => $"An unexpected error occurred with {item}.";
}
