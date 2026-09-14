using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Parameters;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Data;

public partial class Nation : IBlobObject
{


    public void Activate(InitParms initParms)
    {
        GoodsAvailable = GoodStat.GetRandomGoodStat(initParms.InitialNationGoods);
        OrdersState = OrdersState.AwaitOrders;
        // DistrictSetup? home = gameSetup.FindDistrictSetup(HomeDistrict);
        // Population = home?.Population ?? 0;
        // NationReport = new($"Report for {Naming.FormalNation} - {game.GameDate}");
        // NationReport.AddItem($"<b><u>Owned Districts</u></b>: {home?.Name ?? "None"}");
        // EconActivity econActivity = new EconActivity(GoodsAvailable);
        // NationReport.AddTitledList("<b><u>Economics Summary</u></b>:", econActivity.SummarizeEcon());
    }
    // public void CreateReport(Game game)
    // {
    //     NationReport = new($"Report for {Naming.FormalNation} - {game.GameDate}");
    //     List<District> owned = game.Districts.OwnedBy(Identity.NationCode);
    //     NationReport.AddItem($"<b><u>Owned Districts</u></b>: {string.Join(", ", owned.Select(d => d.Name))}");
    //     FoodMetrics? foodMetrics = Economics.Economics.GetFoodMetrics(Population, GoodsAvailable, game.EconParms);
    //     NationReport.AddTitledList("<b><u>Economics Summary</u></b>:", Economics.Economics.SummarizeEcon(GoodsAvailable));
    // }

    public void SeasonUpdate(NationMgr mgr, List<DistrictMgr> myDistricts, List<string> goodNames)
    {
        //  List<DistrictMgr>? withOrder = myDistricts.Where(d => d.Order is not null).ToList();
        //   DistrictOrders = withOrder?.Select(d => d.Order!).ToList();
        Population = myDistricts.Sum(d => d.Population ?? 0);
        OrdersState = OrdersState.AwaitOrders;
        GoodsAvailable = mgr.Final;
        NationReport = mgr.CreateReport("Last Season", myDistricts, goodNames);
        //  CreateReport(myDistricts, mgr, gameDate);
    }
    // public void CreateReport(NationMgr mgr, GameDate? gameDate)
    // {
    //     NationReport = new($"{Naming.FormalNation} - Last Season");
    //     string districtList = string.Join(", ", mgr.DistrictMgrs.Select(d => d.Name));
    //     NationReport.AddItem($"<b><u>Owned Districts</u></b>: ({mgr.DistrictMgrs.Count}) {districtList}");
    //     NationReport.AddItem($"<b><u>Net Population</u></b>: {Population}");
    //     if (mgr?.Adjustments.Count > 0) NationReport.AddTitledList("<b><u>Activity</u></b>:", mgr.Adjustments);
    //     NationReport.AddTitledList("<b><u>Economic Activity</u></b>:", mgr?.SummarizeEcon());

    // }
    public void UpdateNaming(NationNaming newNaming, List<string> leaderTitles, List<string> governments)
    {
        Naming.Name = newNaming.Name;
        Naming.Possessive = newNaming.Possessive ?? newNaming.Name + "n";
        Naming.LeaderTitle = newNaming.LeaderTitle ?? Util.PickRandomFromList(leaderTitles);
        Naming.Government = newNaming.Government ?? Util.PickRandomFromList(governments);
    }
    public void ConfirmPlayerAllowed(PlayerProfile profile)
    {
        if (profile.UserName == PlayerName) return;
        if (profile.RoleLevel >= RoleLevel.Admin) return;
        throw new AppException(ExceptionType.Auth, AuthFailType.UserNotAuthorized, profile.UserName);
    }
    public List<Asset> FoodGoodsProratedPerVillage(int foodPerVillage, List<string> foodTypes)
    {
        int foodAvail = NetFoodAvailable(foodTypes);
        List<Asset> goodsNeeded = [];
        int netGoodsPerVillage = 0;
        foreach (Asset good in GoodsAvailable ?? [])
        {
            if (foodTypes.Contains(good.Name) is false) continue;
            double ratio = (double)good.Amount / foodAvail;
            int goodPerVillage = (int)Math.Floor(ratio * foodPerVillage);
            goodsNeeded.Add(new Asset(good.Name, goodPerVillage));
            netGoodsPerVillage += goodPerVillage;
        }
        int shortfall = foodPerVillage - netGoodsPerVillage;
        int n = 0;
        while (shortfall > 0)
        {
            if (n >= goodsNeeded.Count) n = 0;
            goodsNeeded[n++].Amount += 1;
            shortfall--;
        }
        return goodsNeeded;
    }

    public int NetFoodAvailable(List<string> foodTypes)
    {
        int netFood = 0;
        foreach (Asset asset in GoodsAvailable ?? [])
        {
            if (foodTypes.Contains(asset.Name) is false) continue;
            netFood += asset.Amount;
        }
        return netFood;
    }
}
public static class NationExtensions
{
    public static string NameOf(this List<Nation> nations, int? nationCode)
    {
        return nations.Find(n => n.Identity.NationCode == nationCode)?.Naming.Name ?? "unknown";
    }

}
