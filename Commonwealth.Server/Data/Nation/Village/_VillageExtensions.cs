

using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Server.Data;

public partial class Village
{
    // public static Village Create(VillageIdentity identity)
    // {
    //     Village village = new Village()
    //     {
    //         Order = new VillageOrder(identity),
    //         Count = 0,
    //         Active = 0
    //     };
    //     return village;
    // }

    public void SeasonUpdate(VillageMgr mgr)
    {
        Count += mgr.Order.Build;
        Active += Count + mgr.Order.Activate - mgr.Order.Deactivate;
        Order = new VillageOrder(mgr.Identity);
    }
}
public static class VillageExtensions
{
    public static List<Village> Here(this List<Village> villages, string districtName)
    {
        return villages.Where(v => v.Identity.District == districtName).ToList();
    }
    public static List<Village> OwnedBy(this List<Village> villages, int nationCode)
    {
        return villages.Where(v => v.Identity.Owner == nationCode).ToList();
    }
    public static List<Village> NotOwnedBy(this List<Village> villages, int nationCode)
    {
        return villages.Where(v => v.Identity.Owner != nationCode).ToList();
    }

}