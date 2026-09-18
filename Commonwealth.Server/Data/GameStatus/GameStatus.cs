
using System.Text.Json.Serialization;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using Commonwealth.Shared.Common;

namespace Commonwealth.Server.Data;

// 
public partial class GameStatus : IBlobObject
{
  public int Version { get; set; } = 1; // Both
  public Guid Id { get; set; } // Both
  public required string GameName { get; set; } // Both
  public DateTime? OrdersDueDate { get; set; } // Seasonal
  public GameDate GameDate { get; set; } = default!; // Seasonal
  public GameState GameState { get; set; } // Setup
  public List<DistrictStatus> DistrictStatuses { get; set; } = default!;
  public List<MarketData>? MarketDatas { get; set; } // Seasonal
  public Report? WorldNews { get; set; } // Seasonal
  public Report? GameNews { get; set; } // Seasonal

  [JsonConstructor] public GameStatus() { }

  public static GameStatus Default(string gameName)
  {
    return new GameStatus()
    {
      GameName = gameName,
      GameState = GameState.Created,
    };
  }

}







