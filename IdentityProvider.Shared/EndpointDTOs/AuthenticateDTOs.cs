using System.Text.Json.Serialization;

namespace IdentityProvider.EndpointDTOs;

public class AuthenticateRequest
{
    public AuthenticateRequestType RequestType { get; set; }
    public AuthProfileDTO? AuthProfileDTO { get; set; }
}
public class AuthenticateResponse
{
    public string? Token { get; set; }
}
public class AuthProfileDTO : BaseProfile
{
    public required string Password { get; set; }

}
public partial class BaseProfile
{
    public required string UserName { get; set; }
    public string? Email { get; set; }
    public bool? EmailConfirmed { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    [JsonConstructor] public BaseProfile() { }

}

public enum AuthenticateRequestType
{
    NONE = 0,

    REGISTER = 20,
    AUTHENTICATE = 30,
    UnVerifiedToken = 99,
}

public static class CustomClaims
{
    public const string AuthorityLevel = "authlevel";
    public const string EmailConfirmed = "emailconfirmed";
}