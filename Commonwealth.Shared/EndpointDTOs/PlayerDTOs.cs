using System.Text.Json.Serialization;


namespace Commonwealth.Shared.EndpointDTOs;

public class PlayerRequest : RequestBase
{
    public PlayerRequestType? RequestType { get; set; }
    public List<Guid>? FriendIds { get; set; }
}
public enum PlayerRequestType { NONE = 0, GetPortfolio = 10, GetFriends = 20, AddFriends = 25, RemoveFriends = 29, NewPlayer = 90, RemovePlayer = 99 };

public class PlayerResponse : ResponseBase
{
    public List<GameSummaryDTO>? GameSummaries { get; set; }
   // public string? PlayerName { get; set; }
    public PlayerProfile? PlayerProfile {get;set;}
}

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
public partial class PlayerProfile
{
    //   public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public string? Email { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public int? RoleLevel {get; set;}
    [JsonConstructor] public PlayerProfile() { }

}
