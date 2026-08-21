namespace Commonwealth.Shared.EconomicMgrs;

public static class DistrictMgrFactory
{
     public static DistrictMgr CreateExpansionDistrict(string districtName)
     {
          return new DistrictMgr(districtName, null, 0){};
     }
}