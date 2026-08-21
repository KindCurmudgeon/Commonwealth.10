
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Commonwealth.Shared.Common;
namespace Commonwealth.Server.Parameters;

public static class EconParmsHelpers
{
    public static EconParms LoadEconParms(EconParmsFile? econParmsFile)
    {
        return new EconParms()
        {
            VillageParms = Defaults.VillageParms,
            GoodParms = Defaults.GoodParms,
            Resources = Defaults.Resources,
            SpyParms = Defaults.SpyParms,
            ProductionCapabilityCurve = Defaults.ProductionCapabilityCurve,
            FoodConsumptionPerCapita = Defaults.FoodConsumptionPerCapita,
            FoodRationingThreshhold = Defaults.FoodRationingThreshhold,
            FoodTypes = Defaults.GoodParms.Where(gp => gp.Type == GOODTYPE.FOOD).Select(g => g.Name).ToList(),
            MarketParms = Defaults.MarketParms,
            MinVillagesNeededForDistrictOwnership = Defaults.MinVillages,
            ImmigrationFactor = Defaults.ImmigrationFactor,
            StaffingFoodFactor = Defaults.StaffingFoodFactor,
            StaffingWageFactor = Defaults.StaffingWageFactor
        };
    }
}
// public partial class EconParms
// {
//     [SetsRequiredMembers]
//     public EconParms(EconParmsFile? econParmsFile)
//     {
//         VillageParms = Defaults.VillageParms;
//         GoodParms = Defaults.GoodParms;
//         Resources = Defaults.Resources;
//         SpyParms = econParmsFile?.SpyParms ?? Defaults.SpyParms;
//         ProductionCapabilityCurve = econParmsFile?.ProductionCapabilityCurve ?? Defaults.ProductionCapabilityCurve;
//         FoodConsumptionPerCapita = econParmsFile?.FoodConsumptionPerCapita ?? Defaults.FoodConsumptionPerCapita;
//         FoodRationingThreshhold = econParmsFile?.FoodRationingThreshhold ?? Defaults.FoodRationingThreshhold;
//         FoodTypes = GoodParms.Where(gp => gp.Type == GOODTYPE.FOOD).Select(g => g.Name).ToList();
//         MarketParms = econParmsFile?.MarketParms ?? Defaults.MarketParms;
//         MinVillagesNeededForDistrictOwnership = econParmsFile?.MinVillagesNeededForDistrictOwnership ?? Defaults.MinVillages;
//         ImmigrationFactor = econParmsFile?.ImmigrationFactor ?? Defaults.ImmigrationFactor;
//         StaffingFoodFactor = econParmsFile?.StaffingFoodFactor ?? Defaults.StaffingFoodFactor;
//         StaffingWageFactor = econParmsFile?.StaffingWageFactor ?? Defaults.StaffingWageFactor;
//     }

//     public void Validate()
//     {
//         // List<string> issues = Util.FindAnyNullProperties(this);
//         // if (issues.Count > 0) throw new Exception(Message.MISSINGPARM(String.Join(", ", issues)));
//     }
// }
public static class Defaults
{
    public static List<Resource> Resources = new()
    {
        new() { Name = RESOURCE.FARMLAND, Constraint = null, Existence = 0.7 },
        new() { Name = RESOURCE.WILDFISH, Constraint = Feature.SEACOAST, Existence = 0.9 },
        new() { Name = RESOURCE.FOREST, Constraint = null, Existence = 0.5 },
        new() { Name = RESOURCE.CLAY, Constraint = null, Existence = 0.5 },
        new() { Name = RESOURCE.GEMORE, Constraint = null, Existence = 0.2 }
    };
    public static List<VillageParm> VillageParms = new List<VillageParm>()
    {
            new() {
                Name = VILLAGE.FARMING,
                Type = VILLAGETYPE.PRODUCTION,
                Production = new Asset(GOODS.GRAIN, 20),
                Resource = RESOURCE.FARMLAND,
                Construction = [new Asset(GOODS.WOOD,10),new Asset(GOODS.BRICK, 10), new Asset(GOODS.GEMS, 100)],
                Activation = [new Asset(GOODS.GEMS, 50)],
                Operation = [new Asset(GOODS.WOOD, 10),new Asset(GOODS.GEMS, 5)],

            },
            new() {
                Name = VILLAGE.FISHING,
                Type = VILLAGETYPE.PRODUCTION,
                Production = new Asset(GOODS.FISH, 20),
                Resource = RESOURCE.WILDFISH,
                Construction = [new Asset(GOODS.WOOD,10),new Asset(GOODS.BRICK, 10), new Asset(GOODS.GEMS, 100)],
                Activation = [new Asset(GOODS.GEMS, 50)],
                Operation = [new Asset(GOODS.WOOD, 10),new Asset(GOODS.GEMS, 5)]
            },
                new() {
                Name = VILLAGE.MILLING,
                Type = VILLAGETYPE.PRODUCTION,
                Production = new Asset(GOODS.WOOD, 40),
                Resource = RESOURCE.FOREST,
                Construction = [new Asset(GOODS.WOOD,10),new Asset(GOODS.BRICK, 10), new Asset(GOODS.GEMS, 100)],
                Activation = [new Asset(GOODS.GEMS, 50)],
                Operation = [new Asset(GOODS.WOOD, 10),new Asset(GOODS.GEMS, 5)]
            },
            new() {
                Name = VILLAGE.BRICKMAKING,
                Type = VILLAGETYPE.PRODUCTION,
                Production = new Asset(GOODS.BRICK, 20),
                Resource = RESOURCE.CLAY,
                Construction = [new Asset(GOODS.WOOD,10),new Asset(GOODS.BRICK, 10), new Asset(GOODS.GEMS, 100)],
                Activation = [new Asset(GOODS.GEMS, 50)],
                Operation = [new Asset(GOODS.WOOD, 10),new Asset(GOODS.GEMS, 5)]
            },
            new() {
                Name = VILLAGE.MINING,
                Type = VILLAGETYPE.PRODUCTION,
                Production = new Asset(GOODS.GEMS, 100),
                Resource = RESOURCE.GEMORE,
                Construction = [new Asset(GOODS.WOOD,10),new Asset(GOODS.BRICK, 10), new Asset(GOODS.GEMS, 100)],
                Activation = [new Asset(GOODS.GEMS, 50)],
                Operation = [new Asset(GOODS.WOOD, 10),new Asset(GOODS.GEMS, 5)]
            }
            // new() {
            //     Name = VILLAGE.DEPOT,
            //     Type = VILLAGETYPE.TRADING,
            //     Construction = [new Asset(GOODS.WOOD,10),new Asset(GOODS.BRICK, 10), new Asset(GOODS.CURRENCY, 50)],
            //     Activation = [new Asset(GOODS.WOOD, 5), new Asset(GOODS.CURRENCY, 25)],
            //     Operation = [new Asset(GOODS.WOOD, 10),new Asset(GOODS.CURRENCY, 5)]
            // },
            // new() {
            //     Name = VILLAGE.PORT,
            //     Type = VILLAGETYPE.TRADING,
            //     Construction = [new Asset(GOODS.WOOD, 30), new Asset(GOODS.BRICK, 10),new Asset(GOODS.CURRENCY, 50)],
            //     Activation = [new Asset(GOODS.WOOD, 5), new Asset(GOODS.CURRENCY, 25)],
            //     Operation = [new Asset(GOODS.WOOD, 10),new Asset(GOODS.CURRENCY, 5)]
            // },
    };


    public static List<GoodParm> GoodParms = new()
    {
        new () {Name=GOODS.GRAIN,Type=GOODTYPE.FOOD},
        new () {Name=GOODS.FISH,Type=GOODTYPE.FOOD},
        new () {Name=GOODS.WOOD,Type=GOODTYPE.VILLAGE},
        new () {Name=GOODS.BRICK,Type=GOODTYPE.VILLAGE},
        new () {Name=GOODS.GEMS,Type=GOODTYPE.CURRENCY}
    };
    public static SpyParms SpyParms = new()
    {
        Training = [new Asset("Gems", 20)],
        Operation = [new Asset("Gems", 10)],
        Transfer = [new Asset("Gems", 10)]
    };
    public static decimal TaxRate = 0.2M;
    public static double ActivationCostFactor = 0.5;
    public static double FoodConsumptionPerCapita = 0.01;
    public static int FoodRationingThreshhold = 2;
    public static double ImmigrationFactor = 50.0;
    public static double StaffingFoodFactor = 1.0;
    public static double StaffingWageFactor = 1.0;
    public static int MinVillages = 2;
    public static List<LookupPoint> ProductionCapabilityCurve = new List<LookupPoint>()
    {
            new (0,0.5),
            new (0.01, 0.5),
            new (0.1, 1.0)
    };
    public static MarketParms MarketParms = new MarketParms()
    {
        Commission = 0.1,
        MaxMarketBuyPercent = 0.5
    };

}
