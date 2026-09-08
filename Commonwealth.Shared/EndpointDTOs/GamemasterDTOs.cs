using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;


namespace Commonwealth.Shared.EndpointDTOs;

public class GamemasterRequest : RequestBase
{
    public required GM_RequestType RequestType { get; set; }
    public string? GameName { get; set; }
    public GameDTO? GameDTO { get; set; }
    public bool? UseHistory { get; set; }
    public List<LineupDTO>? LineupDTOs { get; set; } = [];
}
public enum LineupAction { NONE = 0, Added = 10, ChangePlayer = 99, ToBeRemoved = -99 }

public enum GM_RequestType { NONE = 0, Create = 10, Get = 20, Update = 30, Activate = 40, SeasonUpdate = 50, Remove = 99 }
public class GamemasterResponse : ResponseBase
{
    public GameDTO? GameDTO { get; set; }
    public List<LineupDTO>? LineupDTOs { get; set; }
    public bool? IsGamemaster { get; set; }
    public List<string>? Friends { get; set; }
}

public class GameDTO
{
    public string? GameName { get; set; }
    public TimeSpan? OrdersPeriod { get; set; }
    public GameState GameState { get; set; }
    public required string Creator { get; set; }
    public ParmFileInfo? GeogFileInfo { get; set; }
    public ParmFileInfo? EconFileInfo { get; set; }
    public ParmFileInfo? InitFileInfo { get; set; }
    public bool ArchiveGame { get; set; }

    public DateTime? CreationDateTime { get; set; }
    public List<string>? Gamemasters { get; set; }
    public bool? IsActivated { get; set; }
    public string? GameStateString()
    {
        switch (GameState)
        {
            case GameState.None: return "None";
            case GameState.Created: return "Created";
            case GameState.Activated: return "Active";
            case GameState.Completed: return "Completed";
            case GameState.Archived: return "Archived";
            default: return null;
        }
    }
    public GameDTO DeepCopy()
    {
        return new GameDTO()
        {
            GameName = GameName,
            GameState = GameState,
            OrdersPeriod = OrdersPeriod,
            Creator = Creator,
            GeogFileInfo = GeogFileInfo,
            EconFileInfo = EconFileInfo,
            InitFileInfo = InitFileInfo,
            CreationDateTime = CreationDateTime,
            Gamemasters = Gamemasters,
            IsActivated = IsActivated
        };
    }
}

public class LineupDTO
{
    public Guid LineupId { get; set; }
    public required string PlayerName { get; set; }
    public NationIdentity? Identity { get; set; }
    public LineupState LineupState { get; set; }
    public OrdersState OrdersState { get; set; }
    public string? HomeDistrict { get; set; }
    public string? NationName { get; set; }
    public string? NewPlayer { get; set; }

    public string? StateString()
    {
        if (OrdersState == OrdersState.None)
        {
            switch (LineupState)
            {
                case LineupState.Added: return "Added";
                case LineupState.Invited: return "Invited";
                case LineupState.Accepted: return "Accepted";
                case LineupState.Regrets: return "Regrets";
                default: return null;
            }
        }
        switch (OrdersState)
        {
            case OrdersState.AwaitOrders: return "Awaiting Orders Submission";
            case OrdersState.OrdersSubmitted: return "Orders Submitted";
            case OrdersState.Remove: return "Marked for Removal";
            case OrdersState.Replaced: return "Marked for Replacement";
            default: return null;
        }
    }

}

public enum GameState
{
    None = 0, Created = 10, Activated = 100, Completed = 999, Archived = 9999
}
public enum LineupState
{
    NotFound = 0, Added = 10, Invited = 30, Accepted = 40, Regrets = -10
}