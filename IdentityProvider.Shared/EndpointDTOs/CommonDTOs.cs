namespace IdentityProvider.EndpointDTOs;

public static class IdentityEndpoint
{
    public const string Authenticate = "/auth";
    public const string Profile = "/profile";
}

// public class ResponseBase()
// {
//     // public Response Response { get; set; } = new();
//     //     public void HandleException(Exception ex)
//     // {
//     //     // Status = false;
//     //     // ExceptionMessage = ex.Message;
//     //     // Messages.Add(ex.Message);
//     // }
//     // public void AddError(string message)
//     // {
//     //     // Status = false;
//     //     // Messages.Add(message);
//     // }
// }
// public class Response
// {
//     // public bool Status { get; set; } = true;
//     // public List<string> Messages { get; set; } = [];
//     // public string? ExceptionMessage { get; set;}

// }