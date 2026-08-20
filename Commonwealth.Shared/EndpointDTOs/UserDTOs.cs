using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Commonwealth.Shared.EndpointDTOs;

public class UserRequest : RequestBase
{
    public UserRequestType? RequestType { get; set; }
    public UserDTO? UserDTO { get; set; }
    public string? UserName { get; set; }
}
public enum UserRequestType { NONE = 0, CONFIRM = 10, UPDATE = 30, GET_PORTFOLIO = 40, GETUSERDTO = 50, REMOVE = 99 }
public partial class UserResponse : ResponseBase
{
    public List<GameSummaryDTO>? GameSummaries { get; set; }
    public UserDTO? UserDTO { get; set; }
    public string? UserName { get; set; }
}



public partial class UserIdentity
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public bool? IsAdmin { get; set; }
    public bool? IsDeveloper { get; set; }
    [JsonConstructor] public UserIdentity() { }
    [SetsRequiredMembers]
    public UserIdentity(string userName)
    {
        UserName = userName;
        Email = string.Empty;
    }

}
public partial class GameSummaryDTO
{
    public string? GameName { get; set; }
    public GameState? GameState { get; set; }
    public bool IsGamemaster { get; set; }
    public bool? IsDevelopmentGame { get; set; }
    public bool IsCreator { get; set; }
    public string? GameDate { get; set; }
    public int? WaitingCount { get; set; }
    public bool? TimedOut { get; set; }
    public NationSummaryDTO? NationSummary { get; set; }
    public bool ArchiveRequestState { get; set; }
    [JsonConstructor] public GameSummaryDTO() { }
}
public partial class NationSummaryDTO
{
    public required NationIdentity Identity { get; set; }
    public LineupState EntryState { get; set; }
    //   public required string Name { get; set; }
    public NationNaming? Naming { get; set; }
    public string? HomeDistrict { get; set; }
    public OrdersState OrdersState { get; set; }
    [JsonConstructor] public NationSummaryDTO() { }
}


// public partial class ParticipantInfo : NationDTO
// {
//     //public required NationIdentity Identity { get; set; }
//     //public string? NationName { get; set; }
//     //public string? HomeRegion { get; set; }
//     public GameState? GameState { get; set; }
//     // public EntryState? LineupStatus { get; set; }
//     public OrdersState? OrdersState { get; set; }
//     public string? GameDate { get; set; }

//     [JsonConstructor] public ParticipantInfo() { }
// }
// public partial class GamemasterInfo
// {
//     public string? GameName { get; set; }
//     public string? GameDate { get; set; }
//     public GameState? GameState { get; set; }
//     public int? WaitingCount { get; set; }
//     [JsonConstructor] public GamemasterInfo() { }
// }
public partial class UserDTO
{
    public string Password { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string GivenName { get; set; } = string.Empty;
    public List<string> Friends { get; set; } = [];
    [JsonConstructor] public UserDTO() { }
}
