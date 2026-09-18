
using System.Text.Json.Serialization;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using Commonwealth.Shared.Common;
using Commonwealth.Server.Endpoints;

namespace Commonwealth.Server.Data;

public partial class GameSetup : GameAuthority, IBlobObject
{
  public int Version { get; set; } = 1; // BOth
  public Guid Id { get; set; } // Both
  public required string GameName { get; set; } // Both
  public ParmFileInfo? GeogFileInfo { get; set; } // Setup
  public ParmFileInfo? InitFileInfo { get; set; } // Setup
  public ParmFileInfo? EconFileInfo { get; set; } // Setup
    public GameState GameState { get; set; } // Setup

  // public required string CreatorName { get; set; } // Setup
  // public List<string> Gamemasters { get; set; } = []; // Setup
  public DateTime? CreationDate { get; set; } // Setup
  public DateTime? ActivationDate { get; set; } // Setup
  public TimeSpan OrdersPeriod { get; set; } // Setup

  public EconParms EconParms { get; set; } = default!; // Setup
 // public List<DistrictSetup> Districts { get; set; } = []; // Setup
  [JsonConstructor] public GameSetup() { }

}
// public partial class GameSetupBase : GameAuthority
// {
//   public int Version { get; set; } = 1; // BOth
//   public Guid Id { get; set; } // Both
//   public required string GameName { get; set; } // Both
//   public bool? HasPrescribedNationAssignments { get; set; } = true;  // Setup
//   public bool? IsDevelopmentGame { get; set; } = true; // Setup
//   public ParmFileInfo? GeogFileInfo { get; set; } // Setup
//   public ParmFileInfo? InitFileInfo { get; set; } // Setup
//   public ParmFileInfo? EconFileInfo { get; set; } // Setup

//   // public required string CreatorName { get; set; } // Setup
//   // public List<string> Gamemasters { get; set; } = []; // Setup
//   public DateTime? CreationDate { get; set; } // Setup
//   public DateTime? ActivationDate { get; set; } // Setup
//   public TimeSpan OrdersPeriod { get; set; } // Setup

//   public EconParms EconParms { get; set; } = default!; // Setup

//   [JsonConstructor] public GameSetupBase() { }

// }







