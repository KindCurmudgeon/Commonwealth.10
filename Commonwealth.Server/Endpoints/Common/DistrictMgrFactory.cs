
using Commonwealth.Server.Data;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Server.Endpoints;

public static class DistrictMgrFactory
{
     public static List<DistrictMgr> AssembleDistrictMgrs(this List<DistrictStatus> source, WorldSetup worldSetup , List<Village> villages)
     {
          List<DistrictMgr> districtMgrs = [];
          foreach (DistrictStatus status in source)
          {
               string districtName = status.Name;
               int foreignVillageCount = villages.Here(districtName).NotOwnedBy(status.Owner).Count;
               DistrictMgr mgr = new DistrictMgr()
               {
                    Name = status.Name,
                    Owner = status.Owner,
                    Population = status.Population,
                    AllowedVillages = worldSetup.GetDistrict(districtName).AllowedVillages,
                    FoodMetrics = status.FoodMetrics,
                    ForeignVillageCount = foreignVillageCount,
                    LastSeasonGoodsReport = status.LastSeasonGoodsReport,
                    LastSeasonVillageReport = status.LastSeasonVillageReport
               };
               districtMgrs.Add(mgr);
          }
          return districtMgrs;
     }
     public static List<DistrictMgr> AssembleExpansionDistrictMgrs(this List<DistrictStatus> sources, List<Village>? villages, List<Trade>? trades)
     //     List<DistrictStatus> districtStatus,
     //     List<Village>? allVillages,
     //     int nationCode,
     //     List<Trade>? trades)
     {
          List<DistrictMgr> expansionDistrictMgrs = [];
          foreach (DistrictStatus status in sources)
          {
               if (AnyInterestsHere(status.Name))
               {
                    DistrictMgr districtMgr = new DistrictMgr()
                    {
                         Name = status.Name
                    };
                    expansionDistrictMgrs.Add(districtMgr);
               }
          }
          return expansionDistrictMgrs;

          bool AnyInterestsHere(string districtName)
          {
               int myVillagesHereCount = villages?.Count(v => v.Identity.District == districtName) ?? 0;
               if (myVillagesHereCount > 0) return true;
               return trades?.Any(t => t.Order.DistrictName == districtName) ?? false;
          }
     }
}
