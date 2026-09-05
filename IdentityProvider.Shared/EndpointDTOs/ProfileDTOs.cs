using System.Text.Json.Serialization;

namespace IdentityProvider.EndpointDTOs;

public class ProfileRequest
{
    public string? UserName { get; set; }
    public Guid? UserId { get; set; }
}
public class ProfileResponse
{
    public ProfileDTO? ProfileDTO { get; set; }
}
public partial class ProfileDTO
{
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public string? Email { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    [JsonConstructor] public ProfileDTO() { }
}