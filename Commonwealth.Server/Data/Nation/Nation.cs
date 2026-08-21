using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Data;

public partial class Nation : IBlobObject
{
  public Guid Id {get;set;}
  public required NationIdentity Identity { get; set; }
  public required string UserName { get; set; }
  public required NationNaming Naming { get; set; }
  public string? HomeDistrict { get; set; }
  public LineupState LineupState { get; set; }
  public int SeasonCount { get; set; }
  // public required GameDate GameDate { get; set; }
  public OrdersState OrdersState { get; set; }
  public List<Village>? Villages { get; set; }
  public List<Spy>? Spies { get; set; }
  public List<Trade>? Trades { get; set; }
  public List<Asset>? GoodsAvailable { get; set; }

  public int Currency { get; set; }
  public required int Population { get; set; }
  public Report? NationReport { get; set; }

  [JsonConstructor] public Nation() { }

    [SetsRequiredMembers]
    public Nation(Game game, int nationCode, string userName)
    {
      Id = Guid.NewGuid();
        Identity = new NationIdentity(game.Name, nationCode);
        UserName = userName;
        SeasonCount = -1;
        Naming = new NationNaming() { NationCode = nationCode };
        LineupState = LineupState.Invited;
    }

}


