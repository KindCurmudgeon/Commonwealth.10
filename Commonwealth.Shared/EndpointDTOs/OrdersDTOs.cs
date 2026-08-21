using System.Text.Json.Serialization;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Shared.EndpointDTOs;

public class OrdersRequest : RequestBase
{
    public required OrdersRequestType RequestType { get; set; }
    public required NationIdentity Identity { get; set; }
    public int? SeasonCount { get; set; }
    public OrdersState? SubmissionState { get; set; }
    public List<SpyMgr>? UpdatedSpyMgrs { get; set; }
    public List<VillageMgr>? VillageMgrs { get; set; }
    public List<TradeMgr>? TradeMgrs { get; set; }
}
public class OrdersResponse : ResponseBase
{
    public InfoPackage InfoPackage { get; set; } = default!;
    public MgrPackage MgrPackage { get; set; } = default!;
}
public class MgrPackage
{
    public NationMgr NationMgr { get; set; } = default!;
    public List<DistrictMgr> DistrictMgrs { get; set; } = default!;
    public List<DistrictMgr> ExpansionDistrictMgrs { get; set; } = default!;
    public List<VillageMgr> VillageMgrs { get; set; } = default!;
    //  public List<VillageMgr> ForeignVillageMgrs { get; set; } = default!;
    public List<SpyMgr> SpyMgrs { get; set; } = default!;
    public List<TradeMgr> TradeMgrs { get; set; } = default!;
}
public class InfoPackage
{
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
    public string? GameName { get; set; }
    public string? SeasonString { get; set; }
    public void UpdateExpansionTargets(List<DistrictMgr> expansionDistrictMgrs)
    {
        List<string> expansionOptions = new(OwnedDistrictConnections ?? []);
        IEnumerable<string> existingExpansion = expansionDistrictMgrs.Select(n => n.Name);
        expansionOptions.RemoveAll(t => existingExpansion.Contains(t));
        ExpansionTargets = expansionOptions;
    }
}

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

public class Commodity
{
    public required string Name { get; set; }
    public required int Inventory { get; set; }
    public required int Price { get; set; }
}


public enum OrdersState { None = 0, AwaitOrders = 10, OrdersSubmitted = 20, Replaced = 90, Remove = 99 }

public enum OrdersRequestType { NONE = 0, GET = 10, UPDATE = 20 }