// using System.Diagnostics.CodeAnalysis;
// using System.Text.Json.Serialization;
// using Commonwealth.Shared.EndpointDTOs;

// namespace CredentialsDTO;
// public class CredentialsRequest : RequestBase
// {
//     public CredentialsRequestType RequestType { get; set; }
//     public ProfileDTO? ProfileDTO { get; set; }
// }
// public class CredentialsResponse : ResponseBase
// {
//     public UserInfo? UserInfo { get; set; }
//     public string? Token { get; set; }
// }

// public class ProfileDTO
// {
//     public required string UserName { get; set; }
//     public required string Password { get; set; }
//     public string? Email { get; set; }
//     public string? GivenName { get; set; }
//     public string? FamilyName { get; set; }
//     [JsonConstructor] public ProfileDTO() { }
// }
// public class UserInfo
// {
//     public required string UserName { get; set; }
//     public string? Email { get; set; }
//     public string? GivenName { get; set; }
//     public string? FamilyName { get; set; }
//     [JsonConstructor] public UserInfo(){}

// }
// public enum CredentialsRequestType
// {
//     NONE = 0,
//     REGISTER = 10,
//     GETCREDENTIALS = 20
// }
// public static class Endpoint
// {
//  public const string Credentials = "/creds";
// }