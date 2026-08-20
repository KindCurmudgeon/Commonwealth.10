using System.Text.Json.Serialization;

namespace Commonwealth.Shared.Common;

public partial class EconParms
{
    public required List<GoodParm> GoodParms { get; set; }
    public required List<VillageParm> VillageParms { get; set; }
    public required List<Resource> Resources { get; set; }
    public required SpyParms SpyParms { get; set; }
    public required List<string> FoodTypes { get; set; }
    public required List<LookupPoint>? ProductionCapabilityCurve { get; set; }
    //   public required List<string> ProductionVillageTypes { get; set; }
    public required double FoodConsumptionPerCapita { get; set; }
    public required int FoodRationingThreshhold { get; set; }
    public required MarketParms MarketParms { get; set; }
    public required int MinVillagesNeededForDistrictOwnership { get; set; }
    public required double ImmigrationFactor { get; set; }
    public required double StaffingFoodFactor { get; set; }
    public required double StaffingWageFactor { get; set; }
    [JsonConstructor] public EconParms() { }
}
public partial class GoodParm
{
    public required string Name { get; set; }
    public required string Type { get; set; }
    [JsonConstructor] public GoodParm() { }
}
public partial class Resource
{
    public required string Name { get; set; }
    public Feature? Constraint { get; set; }
    public double? Existence { get; set; }
    [JsonConstructor] public Resource() { }
}
public partial class VillageParm
{
    public required string Name { get; set; }
    public required string Type { get; set; }  // Production, Trading, Storage
    public List<Asset>? Construction { get; set; }
    public List<Asset>? Activation { get; set; }
    public List<Asset>? Operation { get; set; }
    public Asset? Production { get; set; }
    public string? Resource { get; set; }
    public Feature? Constraint { get; set; }
    public int Workers { get; set; }
    //  public List<Expense>? Expenses {get;set;}
    [JsonConstructor] public VillageParm() { }
    // public List<Asset>? GetExpense(EXPENSE type)
    // {
    //     return Expenses?.Find(e=>e.Type == type)?.Goods;
    // }
}
public enum EXPENSE { NONE, CONSTRUCTION, ACTIVATION, OPERATION }
public enum VillageClass { NONE, PRODUCTION, TRADING, STORAGE }
public enum VillageConstraint { NONE, SEACOAST, ISLAND }
public partial class SpyParms
{
    public List<Asset>? Training { get; set; }
    public List<Asset>? Operation { get; set; }
    public List<Asset>? Transfer { get; set; }
    [JsonConstructor] public SpyParms() { }

}
public partial class MarketParms
{
    public double? Commission { get; set; }
    public double? MaxMarketBuyPercent { get; set; }
    [JsonConstructor] public MarketParms() { }
}

public record LookupPoint(double X, double Y);

public enum Feature { NONE = 0, SEACOAST = 1, ISLAND = 2, LANDLOCKED = 3, MOUNTAINS = 4, DESERT = 5 }

public static class EconParmsExtensions
{
    public static int GetFoodAmount(this EconParms econParms, List<Asset> goods)
    {
        return econParms.GetFoodAssets(goods).Sum(g => g.Amount);
    }
    public static List<Asset> GetFoodAssets(this EconParms econParms, List<Asset> goods)
    {
        return goods.Where(g => econParms.FoodTypes.Contains(g.Name)).ToList();
    }
    public static int GetFoodConsumed(this EconParms econParms, int population)
    {
        return (int)Math.Floor(population * econParms.FoodConsumptionPerCapita);
    }

}
public static class RESOURCE
{
    public const string FARMLAND = "Farmland";
    public const string WILDFISH = "Fish";
    public const string FOREST = "Forest";
    public const string CLAY = "Clay";
    public const string GEMORE = "GemOre";
}
public static class VILLAGE
{
    public const string FARMING = "Farming";
    public const string FISHING = "Fishing";
    public const string MILLING = "Milling";
    public const string BRICKMAKING = "Brickmaking";
    public const string MINING = "Mining";
    public const string DEPOT = "Depot";
    public const string PORT = "Port";
}
public static class VILLAGETYPE
{
    public const string PRODUCTION = "Production";
    public const string TRADING = "Trading";
    public const string STORAGE = "Storage";
}
public static class GOODS
{
    public const string GRAIN = "Grain";
    public const string FISH = "Fish";
    public const string WOOD = "Wood";
    public const string BRICK = "Brick";
    public const string GEMS = "Gems";
}
public class GOODTYPE
{
    public const string FOOD = "Food";
    public const string VILLAGE = "Village";
    public const string CURRENCY = "Currency";
}