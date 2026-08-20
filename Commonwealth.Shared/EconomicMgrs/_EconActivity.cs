using System.Reflection;
using System.Text.Json.Serialization;
using Commonwealth.Shared.Common;

namespace Commonwealth.Shared.EconomicMgrs;

public class EconActivity
{
    [JsonInclude] public List<Asset>? Initial { get; private set; }
    [JsonInclude] public List<Asset>? Trades { get; private set; }
    [JsonInclude] public List<Asset>? Food { get; private set; }
    [JsonInclude] public List<Asset>? Villages { get; private set; }
    [JsonInclude] public List<Asset>? Production { get; private set; }
    [JsonInclude] public List<Asset>? Spying { get; private set; }
    [JsonInclude] public List<Asset>? Final { get; private set; }
    [JsonInclude] public int? StaffRemaining { get; private set; }
    [JsonConstructor] public EconActivity() { }
    public EconActivity(List<Asset>? goods, int? staffAvailable)
    {
        Initial = goods?.DeepCopy();
        ResetEconActivity(staffAvailable);
    }
    protected void ResetEconActivity(int? staffAvailable = null)
    {
        Trades = [];
        Food = [];
        Villages = [];
        Production = [];
        Spying = [];
        Final = Initial?.DeepCopy();
        StaffRemaining = staffAvailable;
    }

    public bool ConsumeGoods(string accountName, List<Asset>? amount)
    {
        if (amount == null) return false;
        PropertyInfo? accountProperty = GetAccount(accountName);
        if (accountProperty is null) return false;
        List<Asset>? current = (List<Asset>?)accountProperty.GetValue(this);
        (current ?? []).ConsumeWithoutLimits(amount);
        (Final ?? []).ConsumeWithoutLimits(amount);
        return true;
    }
    public bool ConsumeStaff(int? amount)
    {
        if (StaffRemaining is null) return false;
        StaffRemaining -= amount;
        return true;
    }
    public bool AddGoods(string accountName, List<Asset>? goods)
    {
        if (goods == null) return false;
        PropertyInfo? accountProperty = GetAccount(accountName);
        if (accountProperty is null) return false;
        List<Asset>? current = (List<Asset>?)accountProperty.GetValue(this);
        (current ?? []).Accumulate(goods);
        (Final ??= []).Accumulate(goods);
        return true;
    }
    private PropertyInfo? GetAccount(string accountName)
    {
        if (string.IsNullOrWhiteSpace(accountName)) return null;
        PropertyInfo? propertyInfo = GetType().GetProperty(accountName, BindingFlags.Public | BindingFlags.Instance);
        if (propertyInfo is null) return null;
        return propertyInfo;
    }


    public List<RowData> CreateTableRows(List<string>? goodNames)
    {
        List<RowData> rows = [];
        if (HasActivity()) rows.Add(new RowData("Initial", CreateItems(Initial)));
        if (HasContent(Trades)) rows.Add(new RowData("Traded", CreateItems(Trades)));
        if (HasContent(Food)) rows.Add(new RowData("Food", CreateItems(Food)));
        if (HasContent(Villages)) rows.Add(new RowData("Villages", CreateItems(Villages)));
        if (HasContent(Production)) rows.Add(new RowData("Produced", CreateItems(Production)));
        if (HasContent(Spying)) rows.Add(new RowData("Spying", CreateItems(Spying)));
        //if (GoodsTaxPaid is not null && GoodsTaxPaid.IsEmpty() is false) rows.Add(new RowData("Taxes", GoodsTaxPaid.AssetStrings()));
        rows.Add(new RowData("Net", CreateItems(Final)));
        return rows;

        bool HasContent(List<Asset>? assetList)
        {
            if (assetList is null) return false;
            if (assetList.Count == 0) return false;
            return true;
        }
        bool HasActivity()
        {
            if (HasContent(Trades)) return true;
            if (HasContent(Food)) return true;
            if (HasContent(Villages)) return true;
            if (HasContent(Production)) return true;
            if (HasContent(Spying)) return true;
            return false;

        }

        List<string> CreateItems(List<Asset>? assets)
        {
            List<string> items = [];
            foreach (string goodName in goodNames ?? [])
            {
                Asset? asset = assets?.Find(a => a.Name == goodName);
                items.Add(asset?.Amount.ToString() ?? "-");
            }
            return items;
        }


    }

}
public partial class EconomicsMgr
{
    // (int, REASON) DependentConsume(
    //                 int count,
    //                 List<Asset>? cost,
    //                 EconActivity? goodsSource,
    //                 EconActivity? currencySource,
    //                 EconActivity? staffingSource,
    //                 string account,
    //                 int? staffNeeded = null)
    // {
    //     if (count == 0 || cost is null) return (0, REASON.NONE);
    //     if (isClient) return Consume(count, cost, goodsSource, currencySource, staffingSource, account, staffNeeded);
    //     return LimitedConsume(count, cost, goodsSource, currencySource, staffingSource, account, staffNeeded);
    // }
    // (int, REASON) Consume(
    //         int count,
    //         List<Asset> cost,
    //         EconActivity? goodsSource,
    //         EconActivity? currencySource,
    //         EconActivity? staffSource,
    //         string account,
    //         int? staffNeeded)
    // {
    //     while (count > 0)
    //     {
    //         if ((staffSource?.StaffRemaining ?? 0) < (staffNeeded ?? 0)) return (count, REASON.STAFFING);
    //         List<Asset> goodsCost = ExtractAllButTypeFrom(cost, GOODTYPE.CURRENCY);
    //         List<Asset> currencyCost = ExtractTypeFrom(cost, GOODTYPE.CURRENCY);
    //         goodsSource?.ConsumeGoods(account, goodsCost);
    //         currencySource?.ConsumeGoods(account, currencyCost);
    //         staffSource?.ConsumeStaff(staffNeeded);
    //         count--;
    //     }
    //     return (0, REASON.NONE);
    // }
    // (int, REASON) LimitedConsume(
    //                 int count,
    //                 List<Asset> cost,
    //                 EconActivity? goodsSource,
    //                 EconActivity? currencySource,
    //                 EconActivity? staffSource,
    //                 string account,
    //                 int? staffNeeded)
    // {
    //     while (count > 0)
    //     {
    //         List<Asset> goods = ExtractAllButTypeFrom(goodsSource?.Final, GOODTYPE.CURRENCY);
    //         List<Asset> goodsCost = ExtractAllButTypeFrom(cost, GOODTYPE.CURRENCY);
    //         if (goods.IsSufficient(goodsCost) is false) return (count, REASON.GOODS);
    //         List<Asset> currency = ExtractTypeFrom(currencySource?.Final, GOODTYPE.CURRENCY);
    //         List<Asset> currencyCost = ExtractTypeFrom(cost, GOODTYPE.CURRENCY);
    //         if (currency.IsSufficient(currencyCost) is false) return (count, REASON.MONEY);
    //         if ((staffSource?.StaffRemaining ?? 0) < (staffNeeded ?? 0)) return (count, REASON.STAFFING);
    //         goodsSource?.ConsumeGoods(account, goodsCost);
    //         currencySource?.ConsumeGoods(account, currencyCost);
    //         staffSource?.ConsumeStaff(staffNeeded);
    //         count--;
    //     }
    //     return (0, REASON.NONE);
    // }

    //public List<Asset> ExtractCurrency(List<Asset>? source)
    //{
    //    List<Asset> currency = [];
    //    foreach (Asset asset in source ?? [])
    //    {
    //        GoodParm? parm = EconParms.GoodParms.Find(p => p.Name == asset.Name);
    //        if (parm?.Type == GOODTYPE.CURRENCY) currency.Add(asset);
    //    }
    //    return currency;
    //}
    // public List<Asset> ExtractTypeFrom(List<Asset>? source, string goodType)
    // {
    //     List<Asset> result = [];
    //     foreach (Asset asset in source ?? [])
    //     {
    //         GoodParm? parm = EconParms.GoodParms.Find(p => p.Name == asset.Name);
    //         if (parm?.Type == goodType) result.Add(asset);
    //     }
    //     return result;
    // }
    // public List<Asset> ExtractAllButTypeFrom(List<Asset>? source, string goodType)
    // {
    //     List<Asset> goods = [];
    //     foreach (Asset asset in source ?? [])
    //     {
    //         GoodParm? parm = EconParms.GoodParms.Find(p => p.Name == asset.Name);
    //         if (parm?.Type != goodType) goods.Add(asset);
    //     }
    //     return goods;
    // }
    // public List<Asset> GetNeededFood(TradeOrder order)
    // {
    //     DistrictMgr? districtMgr = AllDistrictMgrs.Find(d => d.Name == order.DistrictName);
    //     List<Asset> needed = districtMgr?.FoodMetrics?.AllocatedFoodConsumed ?? [];
    //     List<Asset> available = ExtractTypeFrom(districtMgr?.Initial, GOODTYPE.FOOD);
    //     needed.ConsumeWithoutLimits(available);
    //     needed.ZeroNegatives();
    //     return needed;
    // }
    // public List<Asset> GetNeededVillages(TradeOrder order)
    // {
    //     List<Asset> needed = [];
    //     if (order.SendNeededVillages is true)
    //     {
    //         List<VillageMgr> villageMgrsHere = AllVillageMgrs.FindAll(v => v.Identity.Owner == order.SendingNationCode && v.Identity.District == order.DistrictName);
    //         foreach (VillageMgr villageMgr in villageMgrsHere)
    //         {
    //             VillageParm? parm = EconParms.VillageParms.Find(v => v.Name == villageMgr.Identity.Name);
    //             needed.Accumulate(ExtractAllButTypeFrom(parm?.Activation, GOODTYPE.CURRENCY).MultiplyBy(villageMgr.Order.Activate));
    //             needed.Accumulate(ExtractAllButTypeFrom(parm?.Construction, GOODTYPE.CURRENCY).MultiplyBy(villageMgr.Order.Build));
    //             needed.Accumulate(ExtractAllButTypeFrom(parm?.Operation, GOODTYPE.CURRENCY).MultiplyBy(villageMgr.Active));
    //         }
    //     }
    //     return needed;
    // }
    // public List<Asset>? GetNetSendWithNeeds(TradeOrder order)
    // {
    //     List<Asset> _send = order.Send ?? [];
    //     if (order.SendNeededFood is true)
    //     {
    //         List<Asset> neededFood = GetNeededFood(order);
    //         ZeroType(_send, GOODTYPE.FOOD);
    //         _send.Accumulate(neededFood);
    //     }
    //     if (order.SendNeededVillages is true)
    //     {
    //         List<Asset> neededVillages = GetNeededVillages(order);
    //         ZeroType(_send, GOODTYPE.VILLAGE);
    //         _send.Accumulate(neededVillages);
    //     }
    //     return (_send.IsEmpty()) ? null : _send;
    // }
    // public void ZeroType(List<Asset> source, string goodType)
    // {
    //     foreach (Asset asset in source)
    //     {
    //         string? Type = EconParms.GoodParms.Find(p => p.Type == goodType)?.Type;
    //         if (Type == goodType) asset.Amount = 0;
    //     }
    // }

    // public int GetMarketPrice(Asset asset)
    // {
    //     GoodParm? goodParm = EconParms.GoodParms.Find(g => g.Name == asset.Name);
    //     if (goodParm?.Type == GOODTYPE.CURRENCY) return 1;
    //     double Price = MarketPrices?.Find(m => m.Name == asset.Name)?.Price ?? 0.0;
    //     return (int)(Price * (1 - EconParms.MarketParms.Commission ?? 0));
    //     //    MarketPrice? price = MarketPrices.Find(p => p.Name == asset.Name)
    // }
    // public static string CreateAjustmentString(int amount, ADJTYPE type, REASON reason)
    // {
    //     string typeString = type switch
    //     {
    //         ADJTYPE.ACTIVATION => "activation",
    //         ADJTYPE.OPERATION => "operation",
    //         ADJTYPE.CONSTRUCTION => "construction",
    //         _ => "unknown"
    //     };
    //     string reasonString = reason switch
    //     {
    //         REASON.NONE => "none",
    //         REASON.STAFFING => "staffing",
    //         REASON.GOODS => "goods",
    //         REASON.MONEY => "money",
    //         _ => "unknown"
    //     };
    //     return $"{typeString.Pluralize(amount)} rescinded due to {reasonString}.";
    // }
}

// public enum REASON { NONE, STAFFING, GOODS, MONEY }
// public record Adjustment(int Amount, ADJTYPE Type, REASON Reason);
// public enum ADJTYPE { ACTIVATION, OPERATION, CONSTRUCTION }
// public static class Economics
// {
//     // public static double Lookup(this List<LookupPoint> points, double inputX)
//     // {
//     //     points.Sort((a, b) => a.X.CompareTo(b.X));
//     //     if (inputX < points[0].X) return points[0].Y;
//     //     int last = points.Count - 1;
//     //     if (inputX > points[last].X) return points[last].Y;
//     //     for (int i = 0; i < last; i++)
//     //     {
//     //         var (x1, y1) = points[i];
//     //         var (x2, y2) = points[i + 1];

//     //         if (inputX >= x1 && inputX <= x2)
//     //         {
//     //             // Linear interpolation formula
//     //             return y1 + (inputX - x1) * (y2 - y1) / (x2 - x1);
//     //         }
//     //     }
//     //     return points[last].Y;
//     // }
//     // public static VillageParm? FindVillageParm(this EconParms econParms, string? villageName)
//     // {
//     //     if (villageName is null) return null;
//     //     return econParms.VillageParms.Find(p => p.Name == villageName);
//     // }
//     // public static string? GetGoodType(this List<GoodParm> goodParms, Asset asset)
//     // {
//     //     return goodParms.Find(p => p.Name == asset.Name)?.Type;
//     // }
//     //public static List<string> SummarizeEcon(this EconActivity econActivity, bool ShowNet = false)
//     //{
//     //    List<string> summary = [];
//     //    if (HasNoActivity() is false)
//     //    {
//     //        // if (econActivity.GoodsAvail is not null)
//     //        //     summary.Add($"Initial: {econActivity.GoodsAvail.AssetString()}");
//     //        if (econActivity.Trades is not null && econActivity.Trades.IsEmpty() is false)
//     //            summary.Add($"Traded: {econActivity.Trades.AssetString()}");
//     //        if (econActivity.Villages is not null && econActivity.Villages.IsEmpty() is false)
//     //            summary.Add($"Consumed: {econActivity.Villages.AssetString()}");
//     //        if (econActivity.Production is not null && econActivity.Production.IsEmpty() is false)
//     //            summary.Add($"Produced: {econActivity.Production.AssetString()}");
//     //        //    if (econActivity.GoodsTaxPaid is not null && econActivity.GoodsTaxPaid.IsEmpty() is false)
//     //        //        summary.Add($"Taxes: {econActivity.GoodsTaxPaid.AssetString()}");
//     //    }
//     //    if (ShowNet && econActivity.Final is not null) summary.Add($"Net: {econActivity.Final.AssetString()}");
//     //    return summary;

//     //    bool HasNoActivity()
//     //    {
//     //        return (econActivity.Trades is null || econActivity.Trades.IsEmpty())
//     //            && (econActivity.Villages is null || econActivity.Villages.IsEmpty())
//     //            && (econActivity.Production is null || econActivity.Production.IsEmpty())
//     //      //       && (econActivity.GoodsTaxPaid is null || econActivity.GoodsTaxPaid.IsEmpty())
//     //      ;
//     //    }
//     //}
// }
// // public interface IEconActivity
// // {
// //     public void Reset();
// // }
// // public interface IMgr<T>
// // {
// //     T? Order { get; }
// //     void ChangeOrder(T order);
// //     bool HasChanged { get; set; }
// //     List<string> Adjustments { get; set; }

// // }
// public interface IOrder<T>
// {
//     T? Order { get; } // set to be private
//     void ChangeOrder(T order);
//     T DeepCopy();
// }
// public interface IReport
// {
//     public Report Report { get; set; }
// }
// public interface IEcon
// {
//     public Report? Report { get; set; }
//     public void Projection();
// }