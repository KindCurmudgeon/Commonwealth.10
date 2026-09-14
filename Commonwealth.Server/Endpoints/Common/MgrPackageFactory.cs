using System.Runtime.InteropServices;
using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.MgrFactory;

public static class MgrPackageFactory
{
     public static MgrPackage Create(VGame vGame, Nation nation, List<VDistrict> ownedDistricts, List<Village> villages)
     {
          return new MgrPackage()
          {
               NationMgr = nation.CreateMgr(),
               VillageMgrs = nation.AssembleVillageMgrs(),
               DistrictMgrs = ownedDistricts.AssembleDistrictMgrs(villages),
               ExpansionDistrictMgrs = ownedDistricts.AssembleExpansionDistrictMgrs(villages, nation.Trades),
               SpyMgrs = nation.AssembleSpyMgrs(),
               TradeMgrs = nation.AssembleTradeMgrs(),
          };

          // List<DistrictStatus> GetExpansionDistricts()
          // {
          //      List<DistrictStatus> unowned = new List<DistrictStatus>(gameStatus.DistrictStatuses).NotOwneBy(nation.Identity.NationCode);
          //      List<Village> myVillages = villages.Where(v=>v.Identity.Owner == nation.Identity.NationCode).ToList();
          //      foreach(DistrictStatus districtStatus in unowned)
          //      {
          //           if (AnyInterestsHere(districtStatus.Name))
          //      }

          // }
     }
     // public static bool AnyInterestsHere(string districtName)
     // {
     //      int myVillagesHereCount = allVillages?.Count(v => v.Identity.District == districtName) ?? 0;
     //      if (myVillagesHereCount > 0) return true;
     //      return trades?.Any(t => t.Order.DistrictName == districtName) ?? false;
     // }
}