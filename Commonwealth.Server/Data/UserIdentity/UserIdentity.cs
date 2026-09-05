using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Data;

// public partial class UserIdentity
// {
//     public Guid Id { get; set; }
//     public required string UserName { get; set; }
//     public DateTime CreationTime { get; set; }
//     public required byte[] Salt { get; set; }
//     public required string HashedPassword { get; set; }
//     public string? Email { get; set; }
//     public string? GivenName { get; set; }
//     public string? FamilyName { get; set; }
//     public List<NationIdentity> NationIdentities { get; set; } = default!;
//     public bool IsAdministrator { get; set; }
//     public bool IsDeveloper { get; set; }
//     public List<string> Friends { get; set; } = [];
//     [JsonConstructor] public UserIdentity() { }

//     [SetsRequiredMembers]
//     public UserIdentity(UserDTO registration, bool isAdmin = false)
//     {
//         UserName = registration.UserName;

//         HashedPassword = PasswordCrypto.HashPassword(registration.Password, out byte[] salt);
//         Salt = salt;
//         Id = Guid.NewGuid();
//         CreationTime = DateTime.UtcNow;
//         Email = registration.Email;
//         FamilyName = registration.FamilyName;
//         GivenName = registration.GivenName;
//         IsAdministrator = isAdmin;
//         NationIdentities = [];
//     }

// }





