
using Commonwealth.Server.Data;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Server.Endpoints;

public static class DistrictMgrFactory
{
     public static List<DistrictMgr> AssembleDistrictMgrs(this List<VDistrict> source, List<Village> villages)
     {
          List<DistrictMgr> districtMgrs = [];
          foreach (VDistrict vDistrict in source)
          {
               string districtName = vDistrict.Name;
               int foreignVillageCount = villages.Here(districtName).NotOwnedBy(vDistrict.Owner).Count;
               DistrictMgr mgr = new DistrictMgr()
               {
                    Name = vDistrict.Name,
                    Owner = vDistrict.Owner,
                    Population = vDistrict.Population,
                    AllowedVillages =vDistrict.AllowedVillages,
                    FoodMetrics = vDistrict.FoodMetrics,
                    ForeignVillageCount = foreignVillageCount,
                    LastSeasonGoodsReport = vDistrict.LastSeasonGoodsReport,
                    LastSeasonVillageReport = vDistrict.LastSeasonVillageReport
               };
               districtMgrs.Add(mgr);
          }
          return districtMgrs;
     }
     public static List<DistrictMgr> AssembleExpansionDistrictMgrs(this List<VDistrict> sources, List<Village>? villages, List<Trade>? trades)
     //     List<DistrictStatus> districtStatus,
     //     List<Village>? allVillages,
     //     int nationCode,
     //     List<Trade>? trades)
     {
          List<DistrictMgr> expansionDistrictMgrs = [];
          foreach (VDistrict vDistrict in sources)
          {
               if (AnyInterestsHere(vDistrict.Name))
               {
                    DistrictMgr districtMgr = new DistrictMgr()
                    {
                         Name = vDistrict.Name
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
