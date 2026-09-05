using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

// public static partial class SigninEndpoints
// {
//     public static async Task SignIn(SigninParameters signinParameters, BlobService blobService, SigninResponse response)
//     {
//         Data.UserIdentity user = await Data.UserIdentity.RetrieveAsync(signinParameters.UserName!, blobService);
//         bool verified = PasswordCrypto.VerifyPassword(signinParameters.Password!, user.HashedPassword, user.Salt);
//         if (verified is false) throw new AppException(ExceptionType.Auth, AuthFailType.UserNotAuthorized, signinParameters.UserName ?? "unknown");
//         response.UserDTO = user.CreateUserIdentity();
//         // if (user.IsAdministrator is true) response.Identity.IsAdmin = true;
//         // if (user.IsDeveloper is true) response.Identity.IsDeveloper = true;
//         response.Token = Authorization.GenerateJwtToken(user);
//     }


// }
// public static class AuthStatic
// {
//     public static void AddAuth(this SigninResponse auth, User? user)
//     {
//         if (user is null)
//         {
//             auth.Identity = null;
//             auth.Token = null;
//         }
//         if (user is not null)
//         {
//             auth.Identity = new(user); //.Create(player);
//             //       auth.Token = Validation.CreateToken(player);
//             auth.Token = Authorization.GenerateJwtToken(user);
//         }
//     }
// }



