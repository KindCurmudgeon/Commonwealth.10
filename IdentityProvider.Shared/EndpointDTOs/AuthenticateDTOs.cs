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
public class AuthProfileDTO : ProfileDTO
{
    public required string Password { get; set; }
}

public enum AuthenticateRequestType
{
    NONE = 0,
    REGISTER = 10,
    AUTHENTICATE = 20
}
