using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using IdentityProvider.EndpointDTOs;
using Utilities;


namespace Data;

public partial class UserProfile : ProfileDTO, IBlobObject
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public required byte[] Salt { get; set; }
    public required string HashedPassword { get; set; }

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