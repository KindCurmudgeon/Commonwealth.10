using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Server.Parameters;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Server.Data;

public partial class DistrictStatus 
{
    public required string Name {get;set;} 
    public int Population { get; set; } // Season
    public int DeltaPopulation { get; set; }// Season
    public int Owner { get; set; }// Season
    public List<Asset>? Goods { get; set; }// Season
    public FoodMetrics? FoodMetrics { get; set; } // Season
    public int StaffAvailable { get; set; } // Season
    public Report? LastSeasonGoodsReport { get; set; } // Season
    public Report? LastSeasonVillageReport { get; set; } // Season
    public int? ForeignVillageCount { get; set; } // Season
    [JsonConstructor] public DistrictStatus() {}

    [SetsRequiredMembers]
    public DistrictStatus(DistrictSetup districtSetup, InitParms initParms)
    {
        Name = districtSetup.Name;
        Owner = 0;
        Population = Util.GetRandomInclusive(initParms.InitialDistrictPopulationStat);
        Goods = GoodStat.GetRandomGoodStat(initParms.InitialDistrictGoods);
    }

}


