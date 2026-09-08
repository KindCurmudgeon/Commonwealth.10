
using System.Text.Json.Serialization;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using Commonwealth.Shared.Common;

namespace Commonwealth.Server.Data;

// 
public partial class GameStatus : IBlobObject
{
  public int Version { get; set; } = 1; // BOth
  public Guid Id { get; set; } // Both
  public required string Name { get; set; } // Both
  public DateTime? OrdersDueDate { get; set; } // Seasonal
  public GameDate GameDate { get; set; } = default!; // Seasonal
  public List<MarketData>? MarketDatas { get; set; } // Seasonal
  public Report? WorldNews { get; set; } // Seasonal
  public Report? GameNews { get; set; } // Seasonal

  [JsonConstructor] public GameStatus() { }

}







