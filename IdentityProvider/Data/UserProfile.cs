using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using IdentityProvider.EndpointDTOs;
using Utilities;


namespace Data;

public partial class UserProfile : IBlobObject
{
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public DateTime CreationTime { get; set; }
    public required byte[] Salt { get; set; }
    public required string HashedPassword { get; set; }
    public string? Email { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public bool IsAdministrator { get; set; }
    public bool IsDeveloper { get; set; }
    //  public List<string> Friends { get; set; } = [];
    [JsonConstructor] public UserProfile() { }

    [SetsRequiredMembers]
    public UserProfile(ProfileDTO profileDTO, string hashedPW, byte[] salt)
    {
        UserName = profileDTO.UserName;
        //   HashedPassword = PasswordCrypto.HashPassword(profileDTO.Password, out byte[] salt);
        HashedPassword = hashedPW;
        Salt = salt;
        Id = Guid.NewGuid();
        CreationTime = DateTime.UtcNow;
        Email = profileDTO.Email;
        FamilyName = profileDTO.FamilyName;
        GivenName = profileDTO.GivenName;
        IsAdministrator = false;
        IsDeveloper = false;
    }

}