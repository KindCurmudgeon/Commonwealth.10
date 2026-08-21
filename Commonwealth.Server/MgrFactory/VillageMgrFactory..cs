using System.Text.Json.Serialization;
using Commonwealth.Server.Data;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Server.MgrFactory;

public static class VillageMgrHelpers
{
     public static List<VillageMgr> AssembleVillageMgrs(this List<Nation> nations)
     {
          List<VillageMgr> mgrs = [];
          foreach (Nation nation in nations)
          {
               mgrs.AddRange(nation.AssembleVillageMgrs());
          }
          return mgrs;
     }
     public static List<VillageMgr> AssembleVillageMgrs(this Nation nation)
     {
          List<VillageMgr> mgrs = [];
          foreach (Village village in nation.Villages ?? [])
          {
               mgrs.Add(village.CreateMgr());
          }
          return mgrs;
     }
     public static VillageMgr CreateMgr(this Village village)
     {
          return new VillageMgr(village.Order)
          {
               HasChanged = false,
               Count = village.Count,
               Active = village.Active
          };
     }
}