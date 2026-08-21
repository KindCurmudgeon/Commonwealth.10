using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class UserEndpoints
{
    public static async Task Confirm(string userName, BlobService blobService, UserResponse response)
    {
        User? user = await User.RetrieveAsync(userName,blobService);
        response.UserName = user?.UserName;
        if (user is null) response.AddError($"User '{userName}' not found.");
    }
}