
using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class UserEndpoints
{
    // public static async Task Update(UserDTO userDTO, Data.UserIdentity user, BlobService blobService, UserResponse response)
    // {
    //     {
    //         user.Email = userDTO.Email;
    //         user.GivenName = userDTO.GivenName;
    //         user.FamilyName = userDTO.FamilyName;
    //         if (string.IsNullOrWhiteSpace(userDTO.Password) is false) ChangePassword();
    //         await ParseFriends();
    //         await user.SaveAsync(blobService);
    //         response.UserDTO = user.CreateDTO();
    //         response.AddMessage($"Account for {user.UserName} has been updated!");

    //         void ChangePassword()
    //         {
    //             if (userDTO.Password.Length < 3) throw new AppException(ExceptionType.Auth, AuthFailType.Invalid, "Password");
    //             user.HashedPassword = PasswordCrypto.HashPassword(userDTO.Password, out byte[] salt);
    //             user.Salt = salt;
    //             response.AddMessage($"Password for {user.UserName} has been updated.");
    //         }

    //         async Task ParseFriends()
    //         {
    //             user.Friends = [];
    //             foreach (string friend in userDTO.Friends)
    //             {
    //                 try
    //                 {
    //                     Data.UserIdentity userFriend = await Data.UserIdentity.RetrieveAsync(friend, blobService);
    //                     user.Friends.Add(userFriend.UserName);
    //                 }
    //                 catch { response.AddError($"'{friend}' not found."); }
    //             }
    //         }

    //         // PortfolioResponse response = new();
    //         // Player? requestor = await FeatureEndpoints.GetAuthenticatedUser(user, dbContext, response);
    //         // Player? target = await Player.Retrieve(targetId, dbContext, response);
    //         // if (ValidateRequest() is false) return Results.Ok(response);
    //         // if (Update() is false) return Results.Ok(response);
    //         // await dbContext.CoordinatedUpdateAsync(null, null, response);
    //         // response.Portfolio = await target!.CreatePortfolioResponse(response);
    //         //return Results.Ok(new ResponseBase());

    //         // bool ValidateRequest()
    //         // {
    //         //     if (target is null) return response.AddErrorReturnFalse(Message.NOTFOUND(null, "Player"));
    //         //     if (requestor is null) return false;
    //         //     return user.IsInRole(Roles.Admin) || requestor.Id == target.Id;
    //         // }

    //         // bool Update()
    //         // {
    //         //     if (target is null) return false;
    //         //     if (request.Handle is not null) target.Handle = request.Handle;
    //         //     if (request.Friends is not null) RefreshFriends();
    //         //     if (request.IsRegistered == false) dbContext.Players.Add(target);
    //         //     return true;

    //         //     void RefreshFriends()
    //         //     {

    //         //     }
    //         // }


    //     }
    // }
}

// public partial class UserDTO
// {
//     [SetsRequiredMembers]
//     public UserDTO(User user)
//     {
//         UserName = user.UserName;
//         Password = string.Empty;
//         Email = user.Email ?? string.Empty;
//         FamilyName = user.FamilyName ?? string.Empty;
//         GivenName = user.GivenName ?? string.Empty;
//         Friends = user.Friends;
//     }
// }