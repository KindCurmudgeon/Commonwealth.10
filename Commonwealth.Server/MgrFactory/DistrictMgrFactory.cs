using Commonwealth.Server.Data;
using Commonwealth.Shared.EconomicMgrs;


namespace Commonwealth.Server.MgrFactory;

public static class DistrictMgrFactory
{
    public static List<DistrictMgr> AssembleDistrictMgrs(this List<District> districts, List<Village> villages)
    {
        List<DistrictMgr> mgrs = [];
        foreach (District district in districts)
        {
            mgrs.Add(district.CreateMgr(villages));
        }
        return mgrs;
    }
    public static DistrictMgr CreateMgr(this District district, List<Village> villages)
    {
        int foreignVillageCount = villages.Here(district.Name).NotOwnedBy(district.Owner).Count;
        return new DistrictMgr(district.Name, district.Goods, foreignVillageCount)
        {
            Owner = district.Owner,
            Population = district.Population,
            AllowedVillages = district.AllowedVillages,
            FoodMetrics = district.FoodMetrics,
            ForeignVillageCount = foreignVillageCount,
            LastSeasonGoodsReport = district.LastSeasonGoodsReport,
            LastSeasonVillageReport = district.LastSeasonVillageReport
        };
    }
    public static List<DistrictMgr> AssembleExpansionDistrictMgrs(
            List<District> districts,
            List<Village>? allVillages,
            int nationCode,
            List<Trade>? trades)
    {
        List<DistrictMgr> expansionDistrictMgrs = [];
        List<District> unownedDistricts = districts.Where(d => d.Owner != nationCode).ToList();
        foreach (District district in unownedDistricts)
        {
            if (AnyInterestsHere(district.Name))
            {
                expansionDistrictMgrs.Add(district.CreateExpansionDistrictMgr());
            }
        }
        return expansionDistrictMgrs;

        bool AnyInterestsHere(string districtName)
        {
            int myVillagesHereCount = allVillages?.Count(v => v.Identity.District == districtName) ?? 0;
            if (myVillagesHereCount > 0) return true;
            return trades?.Any(t => t.Order.DistrictName == districtName) ?? false;
        }
    }

    public static DistrictMgr CreateExpansionDistrictMgr(this District district)
    {
        return new DistrictMgr(district.Name, null, 0);
    }
    public static List<Village> AssembleVillages(this List<Nation> nations)
    {
        List<Village> villages = [];
        foreach (Nation nation in nations)
        {
            villages.AddRange(nation.Villages ?? []);
        }
        return villages;
    }
}