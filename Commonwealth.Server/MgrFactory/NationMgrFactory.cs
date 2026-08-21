using Commonwealth.Server.Data;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Server.MgrFactory;

public static class NationMgrHelpers
{
     public static List<NationMgr> AssembleNationMgrs(this List<Nation> nations)
     {
          List<NationMgr> mgrs = [];
          foreach (Nation nation in nations)
          {
               mgrs.Add(nation.CreateMgr());
          }
          return mgrs;
     }
     public static NationMgr CreateMgr(this Nation nation)
     {
          return new NationMgr(nation.Identity, nation.GoodsAvailable, null)
          {
               Naming = nation.Naming,
               OrdersState = nation.OrdersState,
               SeasonCount = nation.SeasonCount,
               Population = nation.Population,
               LastSeasonReport = nation.NationReport,
               Adjustments = [],
          };
     }
}