namespace Commonwealth.Shared.EndpointDTOs;

public class SigninRequest : RequestBase
{
    public SigninRequestType RequestType { get; set; }
    public SigninParameters? SigninParameters { get; set; }
    public UserDTO? UserDTO { get; set; }
}

public class SigninResponse : ResponseBase
{
    public UserIdentity? Identity { get; set; }
    public string? Token { get; set; }
}
public class SigninParameters
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}

public enum SigninRequestType { NONE = 0, REGISTER = 10, SIGNIN = 20 }