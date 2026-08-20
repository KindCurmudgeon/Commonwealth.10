using System.Text.Json.Serialization;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Shared.EndpointDTOs;

public class OrdersRequest : RequestBase
{
    public required OrdersRequestType RequestType { get; set; }
    public required NationIdentity Identity { get; set; }
    // public GameDate? GameDate { get; set; }
    public int? SeasonCount { get; set; }
    public OrdersState? SubmissionState { get; set; }
    public List<SpyOrder>? SpyOrders { get; set; }
    public List<VillageOrder>? VillageOrders { get; set; }
    public List<TradeOrder>? TradeOrders { get; set; }
    [JsonConstructor] public OrdersRequest() { }
}
public class OrdersResponse : ResponseBase
{
    public InfoPackage InfoPackage { get; set; } = default!;
    public MgrPackage MgrPackage { get; set; } = default!;

    [JsonConstructor] public OrdersResponse() { }
}
public partial class MgrPackage
{
    public NationMgr NationMgr { get; set; } = default!;
    public List<DistrictMgr> DistrictMgrs { get; set; } = default!;
    public List<DistrictMgr> ExpansionDistrictMgrs { get; set; } = default!;
    public List<VillageMgr> VillageMgrs { get; set; } = default!;
    //  public List<VillageMgr> ForeignVillageMgrs { get; set; } = default!;
    public List<SpyMgr> SpyMgrs { get; set; } = default!;
    public List<TradeMgr> TradeMgrs { get; set; } = default!;
}
public partial class InfoPackage
{
    //    public NationIdentity? MyNationIdentity { get; set; }
    //    public int MyNationCode { get; set; }
    public Report? WorldNews { get; set; }
    public Report? GameNews { get; set; }
    public List<string>? NationGoodNames { get; set; }
    public List<string>? DistrictGoodNames { get; set; }
    public EconParms EconParms { get; set; } = default!;
    public List<MarketPrice>? MarketPrices { get; set; }
    public List<string>? VillageNames { get; set; }
    public List<NationNaming>? NationNamings { get; set; }
    //  public NationNaming? MyNationNaming { get; set; }
    public List<string> OwnedDistricts { get; set; } = default!;
    public List<string> OwnedDistrictConnections { get; set; } = default!;
    public List<string> ExpansionTargets { get; set; } = default!;
    public List<string> SpyTargets { get; set; } = default!;
    public List<string?> DistrictTradeTargets { get; set; } = default!;
    public List<string>? NationTradeTargets { get; set; } = default!;
    //   public required EconomicsUpdater EconomicsUpdater { get; set; }
    //   public required Action<bool> OrdersChanged { get; set; }
    public string? GameName { get; set; }
    public string? SeasonString { get; set; }
    [JsonConstructor] public InfoPackage() { }
    public void UpdateExpansionTargets(List<DistrictMgr> expansionDistrictMgrs)
    {
        List<string> expansionOptions = new(OwnedDistrictConnections ?? []);
        IEnumerable<string> existingExpansion = expansionDistrictMgrs.Select(n => n.Name);
        expansionOptions.RemoveAll(t => existingExpansion.Contains(t));
        ExpansionTargets = expansionOptions;
    }


}
//public partial class SpyDTO
//{
//    public required SpyOrder Orders { get; set; }
//    public string? District { get; set; }
//    public Report? Report { get; set; }
//}
//public class VillageDTO
//{
//       public required VillageOrder Orders { get; set; }
//    public string? District { get; set; }
//    public int Population { get; set; }
//    public List<Asset>? Goods { get; set; }
//    public List<Feature>? Features { get; set; }
//    public List<string>? Resources { get; set; }
//    public Report? Report { get; set; }
//    [JsonConstructor] VillageDTO() { }
//}
public static class OrdersExtensions
{
    public static string? NameOf(this List<NationNaming> nationNamings, int nationCode)
    {
        return nationNamings.Find(n => n.NationCode == nationCode)?.Name;
    }
    public static int? CodeOf(this List<NationNaming> nationNamings, string? nationName)
    {
        return nationNamings.Find(n => n.Name == nationName)?.NationCode;
    }
}
//public partial class DistrictDTO
//{
//    public required DistrictOrders Orders { get; set; }
//    public int? Owner { get; set; }
//    public bool IsOwnedByMe { get; set; }
//    public int Population { get; set; }
//    public List<Asset>? Goods { get; set; }
//    public List<Feature>? Features { get; set; }
//    public List<string>? Resources { get; set; }
//    public List<string>? AllowedVillages { get; set; }
//    public Report? Report { get; set; }
//    [JsonConstructor] DistrictDTO() { }
//}
//public partial class NationDTO
//{
//    public required NationIdentity Identity { get; set; }
//    public required NationNaming Naming { get; set; }
//    public string? HomeDistrict { get; set; }
//    public List<Asset>? GoodsAvailable { get; set; }
//    public int Population { get; set; }
//    public Report? Report { get; set; }
//    [JsonConstructor] public NationDTO() { }

//}

//public partial class GameInfoRef
//{
//    public required string GameName { get; set; }
//    public GameDate? GameDate { get; set; }
//    public List<NationNamingDTO>? NationDTOs { get; set; }
//    public EconParms? EconParms { get; set; }
//    public List<MarketData>? MarketDatas { get; set; }
//    public Report? WorldNews { get; set; }
//    [JsonConstructor] public GameInfoRef() { }
//}


//[method: SetsRequiredMembers]
// public class DistrictOrders(string districtName, double taxRate)
// {
//     public required string DistrictName { get; set; } = districtName;
//     public double TaxRate { get; set; } = taxRate;
// }
//public partial class ForeignRef
//{
//    public required List<string> ExpansionDistricts { get; set; }
//    public required List<string> SpyTargets { get; set; }
//    [JsonConstructor] ForeignRef() { }
//}



public partial class Commodity
{
    public required string Name { get; set; }
    public required int Inventory { get; set; }
    public required int Price { get; set; }
    [JsonConstructor] public Commodity() { }

}


public enum OrdersState { None = 0, AwaitOrders = 10, OrdersSubmitted = 20, Replaced = 90, Remove = 99 }

public enum OrdersRequestType { NONE = 0, GET = 10, UPDATE = 20 }