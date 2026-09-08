using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Azure.Storage.Blobs.Models;
using IdentityProvider.EndpointDTOs;
using Utilities;


namespace Data;

public partial class UserProfile : BaseProfile, IBlobObject

{
    public int Version { get; set; } = 1;
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public required byte[] Salt { get; set; }
    public required string HashedPassword { get; set; }
    public int AuthorityLevel { get; set; }

    [JsonConstructor] public UserProfile() { }

    [SetsRequiredMembers]
    public UserProfile(AuthProfileDTO profileDTO, string hashedPW, byte[] salt)
    {
        UserName = profileDTO.UserName;
        HashedPassword = hashedPW;
        Salt = salt;
        Id = Guid.NewGuid();
        CreationTime = DateTime.UtcNow;
        Email = profileDTO.Email;
        FamilyName = profileDTO.FamilyName;
        GivenName = profileDTO.GivenName;
        AuthorityLevel = 0;
    }

}