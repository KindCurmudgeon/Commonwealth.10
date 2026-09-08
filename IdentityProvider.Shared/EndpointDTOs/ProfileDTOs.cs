// using System.Text.Json.Serialization;

// namespace IdentityProvider.EndpointDTOs;

// public class ProfileRequest
// {
//     public string? UserName { get; set; }
// }
// public class ProfileResponse
// {
//     public UserProfileDTO? ProfileDTO { get; set; }
// }
// public partial class UserProfileDTO
// {
//     public required string UserName { get; set; }
//     public string? Email { get; set; }
//     public bool? EmailConfirmed { get; set; }
//     public string? GivenName { get; set; }
//     public string? FamilyName { get; set; }
//     public int RoleLevel { get; set; }
//     [JsonConstructor] public UserProfileDTO() { }

// }
// public enum RoleLevel { BASE, Admin = 10, Developer = 100 }