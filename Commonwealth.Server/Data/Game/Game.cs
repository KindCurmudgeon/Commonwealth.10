
using System.Text.Json.Serialization;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using Commonwealth.Shared.Common;

namespace Commonwealth.Server.Data;

// 
public partial class Game : IBlobObject
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public bool? HasPrescribedNationAssignments {get;set;}= true;
    public bool? IsDevelopmentGame {get;set;} = true;
    public ParmFileInfo? GeogFileInfo { get; set; }
    public ParmFileInfo? InitFileInfo { get; set; }
    public ParmFileInfo? EconFileInfo { get; set; }
    public GameState GameState { get; set; }
    public string? Creator { get; set; }
    public List<string> Gamemasters { get; set; } = [];
    public DateTime? CreationDate { get; set; }
    public DateTime? ActivationDate { get; set; }
    public DateTime? OrdersDueDate { get; set; }
    public TimeSpan OrdersPeriod { get; set; }
    public GameDate GameDate {get;set;} = default!;
    public List<District> Districts { get; set; } = [];
    // public List<Village> Villages { get; set; } = [];
    public List<MarketData>? MarketDatas { get; set; }
    public EconParms EconParms { get; set; } = default!;
  //  public required InitParms InitParms { get; set; }
    public Report? WorldNews { get; set; }
    public Report? GameNews {get;set;}


    [JsonConstructor] public Game() { }

      public Dictionary<string, Game>? UnmappedProperties { get; set; }
}







