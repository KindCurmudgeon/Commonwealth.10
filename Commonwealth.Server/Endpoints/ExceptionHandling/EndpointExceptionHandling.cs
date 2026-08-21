using System.Diagnostics;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints.ExceptionHandling;

public static class EndpointExceptionHandling
{
    public static void HandleEndpointException(this ResponseBase response, AppException ex)
    {
        string failedItem = ex.FailedItem ?? "unknown";

        switch (ex.FailType)
        {
            case EndpointFailType.MissingData:
                response.AddError(Message.MissingData(failedItem));
                break;
            case EndpointFailType.NameExists:
                response.AddError(Message.NameExists(failedItem));
                break;
            case EndpointFailType.SeasonUpdated:
                response.AddError(Message.SeasonUpdated());
                break;
            case EndpointFailType.NotImplemented:
                response.AddError(Message.NotImplemented(failedItem));
                break;
            case EndpointFailType.FailedUpload:
                response.AddError(Message.FailedUpload(failedItem));
                break;
            case EndpointFailType.Invalid:
                response.AddError(Message.Invalid(failedItem));
                break;
            default:
                response.AddError(Message.Unexpected("Endpoint Failure"));
                break;
        }
        response.Response.ExceptionMessage = ex.Message;
        Debug.WriteLine(ex.Message);
    }
}
public static class EndpointFailType
{
    public const string MissingData = "MissingData";
    public const string NameExists = "NameExists";
    public const string SeasonUpdated = "SeasonUpdated";
    public const string NotImplemented = "NotImplemented";
    public const string FailedUpload = "FailedUpload";
    public const string Invalid = "FailedInvalid";

}