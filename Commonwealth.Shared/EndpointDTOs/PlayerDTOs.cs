using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;


namespace Commonwealth.Shared.EndpointDTOs;

public class PlayerRequest : RequestBase
{
    public PlayerRequestType? RequestType { get; set; }
    public List<Guid>? FriendIds { get; set; }
    // public ProfileDTO? UserDTO { get; set; }
    //public string? UserName { get; set; }
}
//public enum UserRequestType { NONE = 0, CONFIRM = 10, UPDATE = 30, GET_PORTFOLIO = 40, GETUSERDTO = 50, REMOVE = 99 }
public enum PlayerRequestType { NONE = 0, GetPortfolio = 10, GetFriends = 20, AddFriends = 25, RemoveFriends = 29, NewPlayer = 90, RemovePlayer = 99 };

public class PlayerResponse : ResponseBase
{
    public List<GameSummaryDTO>? GameSummaries { get; set; }
    public string? PlayerName { get; set; }
    // public string? UserName { get; set; }
}

// public class UserIdentity
// {
//     public required string UserName { get; set; }
//     public required string Email { get; set; }
//     public bool? IsAdmin { get; set; }
//     public bool? IsDeveloper { get; set; }

// }
public class GameSummaryDTO
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
}
public class NationSummaryDTO
{
    public required NationIdentity Identity { get; set; }
    public LineupState EntryState { get; set; }
    public NationNaming? Naming { get; set; }
    public string? HomeDistrict { get; set; }
    public OrdersState OrdersState { get; set; }
}

// public class UserDTO
// {
//     public string Password { get; set; } = string.Empty;
//     public string UserName { get; set; } = string.Empty;
//     public string Email { get; set; } = string.Empty;
//     public string FamilyName { get; set; } = string.Empty;
//     public string GivenName { get; set; } = string.Empty;
//     public List<string> Friends { get; set; } = [];
// }
