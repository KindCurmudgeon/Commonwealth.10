using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public partial class UserEndpoints
{
    public static async Task ValidateAccount(UserRequest request, BlobService blobService, UserResponse response)
    {
        User user = await Authorization.ValidateUserAsync(request, blobService);
        response.UserDTO = user.CreateDTO();
    }
}