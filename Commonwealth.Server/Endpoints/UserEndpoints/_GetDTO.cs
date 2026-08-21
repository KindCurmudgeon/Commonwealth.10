using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;public partial class UserEndpoints
{
    public static async Task GetUserDTO(string userName, BlobService blobService, UserResponse response)
    {
        User user = await User.RetrieveAsync(userName, blobService);
        response.UserDTO = user.CreateDTO();
    }
}