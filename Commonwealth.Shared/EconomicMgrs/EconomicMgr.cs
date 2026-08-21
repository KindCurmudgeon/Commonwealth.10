using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Shared.Common;

namespace Commonwealth.Shared.EconomicMgrs;

public partial class EconomicMgr
{
     public List<NationMgr> AllNationMgrs { get; set; } = [];
     public List<DistrictMgr> AllDistrictMgrs { get; set; } = [];
     public List<VillageMgr> AllVillageMgrs { get; set; } = [];
     public List<TradeMgr> AllTradeMgrs { get; set; } = [];
     public List<SpyMgr> AllSpyMgrs { get; set; } = [];
     public required EconParms EconParms { get; set; }
     public List<MarketPrice> MarketPrices { get; set; } = [];
     public bool isClient { get; set; } = false;
     [JsonConstructor] public EconomicMgr() { }
     [SetsRequiredMembers] public EconomicMgr(EconParms econParms)
     {
          EconParms = econParms;
     }
}

// public enum REASON { NONE, STAFFING, GOODS, MONEY }
// public record Adjustment(int Amount, ADJTYPE Type, REASON Reason);
// public enum ADJTYPE { ACTIVATION, OPERATION, CONSTRUCTION }
// public interface IEconActivity
// {
//      public void Reset();
// }
// public interface IMgr<T>
// {
//      T? Order { get; }
//      void ChangeOrder(T order);
//      bool HasChanged { get; set; }
//      List<string> Adjustments { get; set; }

// }