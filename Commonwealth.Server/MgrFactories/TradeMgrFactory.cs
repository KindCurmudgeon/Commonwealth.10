using Commonwealth.Server.Data;
using Commonwealth.Shared.EconomicMgrs;


namespace Commonwealth.Server.MgrFactory;

public static class TradeMgrHelpers
{
     public static List<TradeMgr> AssembleTradeMgrs(this List<Nation> nations)
     {
          List<TradeMgr> mgrs = [];
          foreach (Nation nation in nations)
          {
               mgrs.AddRange(nation.AssembleTradeMgrs());
          }
          return mgrs;
     }
     public static List<TradeMgr> AssembleTradeMgrs(this Nation nation)
     {
          List<TradeMgr> mgrs = [];
          foreach (Trade trade in nation.Trades ?? [])
          {
               mgrs.Add(trade.CreateMgr());
          }
          return mgrs;
     }
     public static TradeMgr CreateMgr(this Trade trade)
     {
          return new TradeMgr(trade.Order)
          {
               HasChanged = false,
               Adjustments = [],
          };
     }
}
