using Commonwealth.Server.Data;
using Commonwealth.Shared.EconomicMgrs;


namespace Commonwealth.Server.MgrFactory;

public static class SpyMgrFactory
{
     public static List<SpyMgr> AssembleSpyMgrs(this List<Nation> nations)
     {
          List<SpyMgr> mgrs = [];
          foreach (Nation nation in nations)
          {
               mgrs.AddRange(nation.AssembleSpyMgrs());
          }
          return mgrs;
     }
     public static List<SpyMgr> AssembleSpyMgrs(this Nation nation)
     {
          List<SpyMgr> mgrs = [];
          foreach (Spy spy in nation.Spies ?? [])
          {
               mgrs.Add(spy.CreateMgr(nation.Identity.NationCode));
          }
          return mgrs;
     }
     public static SpyMgr CreateMgr(this Spy spy, int nationCode)
     {
          return new SpyMgr(spy.Order)
          {
               NationCode = nationCode,
               District = spy.District,
               Report = spy.Report
          };
     }
     
}