
using System.Text.Json.Serialization;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using Commonwealth.Shared.Common;

namespace Commonwealth.Server.Data;

// 
public partial class Game : IBlobObject
{
  public int Version { get; set; } = 1; // BOth
  public Guid Id { get; set; } // Both
  public required string Name { get; set; } // Both
  public bool? HasPrescribedNationAssignments { get; set; } = true;  // Setup
  public bool? IsDevelopmentGame { get; set; } = true; // Setup
  public ParmFileInfo? GeogFileInfo { get; set; } // Setup
  public ParmFileInfo? InitFileInfo { get; set; } // Setup
  public ParmFileInfo? EconFileInfo { get; set; } // Setup
  public GameState GameState { get; set; } // Setup
  public required string CreatorName { get; set; } // Setup
  public List<string> Gamemasters { get; set; } = []; // Setup
  public DateTime? CreationDate { get; set; } // Setup
  public DateTime? ActivationDate { get; set; } // Setup
  public DateTime? OrdersDueDate { get; set; } // Seasonal
  public TimeSpan OrdersPeriod { get; set; } // Setup
  public GameDate GameDate { get; set; } = default!; // Seasonal
  public List<District> Districts { get; set; } = []; // Setup
  // public List<Village> Villages { get; set; } = [];
  public List<MarketData>? MarketDatas { get; set; } // Seasonal
  public EconParms EconParms { get; set; } = default!; // Setup
  //  public required InitParms InitParms { get; set; }
  public Report? WorldNews { get; set; } // Seasonal
  public Report? GameNews { get; set; } // Seasonal

  [JsonConstructor] public Game() { }

}







