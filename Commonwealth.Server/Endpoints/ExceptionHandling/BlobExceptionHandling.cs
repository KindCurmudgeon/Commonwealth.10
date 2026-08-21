using System.Diagnostics;
using Commonwealth.Shared.EndpointDTOs;
using Commonwealth.Server.Endpoints;

namespace Commonwealth.Server.Endpoints.ExceptionHandling;

public static class BlobExceptionHandling
{
    public static void HandleBlobException(this ResponseBase response, AppException ex)
    {
        string failedItem = ex.FailedItem ?? "unknown";

        switch (ex.FailType)
        {
            case BlobFailType.NotFound:
                response.AddError(Message.NotFound(failedItem));
                break;
            case BlobFailType.JSON:
            case BlobFailType.FileContent:
                response.AddError(Message.FileContent(failedItem));
                break;
            default:
                response.AddError(Message.Unexpected("Blob Failure"));
                break;
        }
        response.Response.ExceptionMessage = ex.Message;
        Debug.WriteLine(ex.Message);
    }
}

public static class BlobFailType
{
    public const string NotFound = "NOTFOUND";
    public const string JSON = "JSON";
    public const string TroubleWrite = "TroubleWrite";
    public const string Unexpected = "Unexpected";
    public const string Remove = "Remove";
    public const string BlobFailure = "BlobFailure";
    public const string FileContent = "FileContent";
}