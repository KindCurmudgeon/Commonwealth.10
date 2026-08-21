using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Data;

public partial class User
{
   public Guid Id { get; set; }
    public required string UserName { get; set; }
   public DateTime CreationTime { get; set; }
    public required byte[] Salt { get; set; }
    public required string HashedPassword { get; set; }
    public string? Email { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public List<NationIdentity> NationIdentities { get; set; } = default!;
    public bool IsAdministrator { get; set; }
    public bool IsDeveloper { get; set; }
    // public required List<NationIdentity> ParticipantGames { get; set; }
    // public required List<string> GamemasterGames { get; set; }
    public List<string> Friends { get; set; } = [];
    [JsonConstructor] public User() { }
    // public static User? Create(UserDTO registration, bool isAdmin = false)
    // {
    //     if (registration.Password is null) return null;
    //     string Hash = PasswordCrypto.HashPassword(registration.Password, out byte[] salt);

    //     return new User()
    //     {
    //         UserName = registration.UserName,
    //         Salt = salt,
    //         HashedPassword = Hash,
    //         Email = registration.Email,
    //         FamilyName = registration.FamilyName,
    //         GivenName = registration.GivenName,
    //         IsAdministrator = isAdmin,
    //         NationIdentities = []
    //     };
    // }
    [SetsRequiredMembers]
    public User(UserDTO registration, bool isAdmin = false)
    {
        UserName = registration.UserName;

        HashedPassword = PasswordCrypto.HashPassword(registration.Password, out byte[] salt);
        Salt = salt;
       Id = Guid.NewGuid();
       CreationTime = DateTime.UtcNow;
        Email = registration.Email;
        FamilyName = registration.FamilyName;
        GivenName = registration.GivenName;
        IsAdministrator = isAdmin;
        NationIdentities = [];
        //    ParticipantGames = [];
        //    GamemasterGames = [];
    }

}
// public partial class GameRole : NationIdentity
// {
//     public required GameRoleType RoleType { get; set; }
//     [JsonConstructor] public GameRole() { }
//     [SetsRequiredMembers]
//     public GameRole(string gameName, int nationCode = 0) : base(gameName, nationCode)
//     {
//         RoleType = roleType;
//     }
//     public bool IsSameAs(GameRole gameRole)
//     {
//         if (GameName != gameRole.GameName) return false;
//         if (NationCode != gameRole.NationCode) return false;
//         return true;
//     }
// }




