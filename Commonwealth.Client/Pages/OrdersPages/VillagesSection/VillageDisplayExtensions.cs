using System.Text;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Client.Pages;
public static class VillageDisplayExtensions
{
    public static string Summary(this VillageMgr result, string? owner = null)
    {
        VillageOrder orders = result.Order;
        string countString = result.Count == 0 ? "No" : result.Count.ToString();
        StringBuilder builder = new();
        builder.Append($"{countString} {owner} {result.Identity.Name} ");
        builder.Append((result.Count == 1) ? "Village" : "Villages");
        if (result.Count != result.Active) builder.Append($" ({result.Active} Active)");
        builder.Append("; ");
        return builder.ToString();
    }
    public static string? Activity(this VillageMgr result)
    {
        VillageOrder orders = result.Order;
        if (orders.Build > 0) return $"\u21D2 {result.Count + orders.Build}";
        if (orders.Activate > 0 && result.Active + orders.Activate == result.Count) return $"\u21D2 All Active";
        if (orders.Activate > 0) return $"\u21D2 {result.Active + orders.Activate} Active";
        if (orders.Deactivate > 0) return $"\u21D2 {result.Active - orders.Deactivate} Active";
        return null;
    }
    public static CellData SummaryCell(this VillageMgr result)
    {
        VillageOrder villageOrders = result.Order;
        string contents = "";
        string? activity = Activity();
        if (result.Count == 0)
        {
            contents = (activity is null) ? "-" : "";
        }
        else
        {
            contents = (result!.Count == result.Active) ? result.Count.ToString() :
            $"{result.Active}/{result.Count}";
        }
        return new CellData(contents, Activity(), true);

        string? Activity()
        {
            if (villageOrders.Build > 0) return $"\u21D2{result.Count + villageOrders.Build}";
            if (villageOrders.Activate > 0)
            {
                if (result.Active + villageOrders.Activate == result.Count) return $"\u21D2{result.Count}";
                return $"\u21D2{result.Active + villageOrders.Activate}/{result.Count}";
            }
            if (villageOrders.Deactivate > 0) return $"\u21D2{result.Active - villageOrders.Deactivate}/{result.Count}";
            return null;
        }
    }
}
public record CellData(string? Contents, string? Activity = null, bool Click = false);