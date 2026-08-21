using System.Text.Json.Serialization;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EndpointDTOs;



namespace Commonwealth.Server.Parameters;

public partial class EconParmsFile : IBlobObject
{
    public required ParmFileInfo ParmFileInfo { get; set; }
    public required List<GoodParmFile> GoodParms { get; set; }
    public required List<Resource> Resources { get; set; }
    public required List<VillageParmFile>? VillageParms { get; set; }
    public required SpyParms? SpyParms { get; set; }
    public required decimal TaxRate { get; set; }
    public required double ActivationCostFactor { get; set; }
    public required double FoodConsumptionPerCapita { get; set; }
    public required int FoodRationingThreshhold { get; set; }
    public required List<LookupPoint>? ProductionCapabilityCurve { get; set; }

    public int? MinVillagesNeededForDistrictOwnership { get; set; }
    public double? ImmigrationFactor { get; set; }
    public double? StaffingFoodFactor {get;set;}
    public double? StaffingWageFactor {get;set;}

    //    public required List<string> FoodTypes { get; set; }
    //    public required List<string> FoodProducingVillageTypes { get; set; }

    public required MarketParms MarketParms { get; set; }
 //   public List<MarketCommodityFile>? InitialMarket { get; set; }
 //   public List<Asset>? InitialDistrictGoods { get; set; }
    [JsonConstructor] public EconParmsFile() { }

}