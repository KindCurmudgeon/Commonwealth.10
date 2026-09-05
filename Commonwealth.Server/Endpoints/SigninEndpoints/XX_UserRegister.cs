
using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

// public static partial class SigninEndpoints
// {
//     public static async Task Register(UserDTO registrationDTO, BlobService blobService, SigninResponse response)
//     {
//         Data.UserIdentity user = new(registrationDTO!);
//         await user.SaveAsync(blobService);
//         response.UserDTO = user.CreateUserIdentity();
//         response.Token = Authorization.GenerateJwtToken(user);
//         response.AddMessage($"{user.UserName} has been registered!");
//     }
// }