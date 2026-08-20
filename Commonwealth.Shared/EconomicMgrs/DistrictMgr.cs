using System.ComponentModel;
using System.Text.Json.Serialization;
using Commonwealth.Shared.Common;

namespace Commonwealth.Shared.EconomicMgrs;

public partial class DistrictMgr : DistrictInfo, IEconActivity
{
    public bool HasChanged { get; set; } = false;
    public required string Name { get; set; }
    public Report? LastSeasonGoodsReport { get; set; }
    public Report? LastSeasonVillageReport { get; set; }
    //  public DistrictOrder Order { get; private set; }
    public List<string> Adjustments { get; set; } = [];

    [JsonConstructor] public DistrictMgr() { }
    public static DistrictMgr CreateExpansion(string name)
    {
        return new DistrictMgr() { Name = name };
    }
    public void Reset()
    {
        ResetEconActivity(StaffAvailable);
    }
    public Report CreateGoodsReport(string? appendix, List<string>? goodNames)
    {
        Report report = new Report($"{Name} Goods - {appendix}");
        switch (FoodMetrics?.FoodStatus)
        {
            case FoodStatus.RATIONING:
                report.AddTextEntry("Food Rationing is in Effect!");
                break;
            case FoodStatus.FAMINE:
                report.AddTextEntry("FAMINE!!!");
                break;
        }
        ReportTableData tableData = new ReportTableData("Activity")
        {
            FirstHeading = "",
            ColumnHeadings = goodNames ?? [],
            RowDatas = CreateTableRows(goodNames)
        };
        report.AddTable(tableData);
        return report;
    }
    public Report CreateVillageReport(string? appendix)
    {
        Report report = new Report($"{Name} Villages - {appendix}");
        report.AddTextEntry($"Population: {Population}");
        int unemployed = (Population ?? 0) - ((int)StaffAvailable - (int)(StaffRemaining ?? 0));
        if (Population is not null && Population > 0)
        {
            int pcu = (int)(unemployed / (Population)) * 100;
            report.AddTextEntry($"Unemployed: {unemployed}  ({pcu}%)");
        }
        return report;
    }
}
public class DistrictInfo : EconActivity
{
    public int? Owner { get; set; }
    public int? Population { get; set; }
    public List<string>? AllowedVillages { get; set; }
    public FoodMetrics? FoodMetrics { get; set; }
    public int StaffAvailable { get; set; }
    public int ForeignVillageCount { get; set; }
    public Report? LastSeasonReport { get; set; }

    [JsonConstructor] public DistrictInfo() : base() { }

    public DistrictInfo(List<Asset>? goods, int staffAvailable) : base(goods, staffAvailable) { }

}
public class FoodMetrics
{
    public List<Asset>? AllocatedFoodConsumed { get; set; } = [];
    public double SeasonsOfFood { get; set; }
    public double ProductionCapacity { get; set; }
    public FoodStatus FoodStatus { get; set; }
    public int FoodNeeded { get; set; }
    public int FoodConsumed { get; set; }
    public string? Summary { get; set; }
    [JsonConstructor] public FoodMetrics() { }

}
public enum FoodStatus
{
    NONE = 0,
    OK = 1,
    [Description("Food Rationing")] RATIONING = 2,
    [Description("Famine")] FAMINE = 3
}
public partial class EconomicMgr
{
    static void HandleFoodConsumption(DistrictMgr mgr)
    {
        List<Asset>? foodConsumed = mgr.FoodMetrics?.AllocatedFoodConsumed ?? [];
        mgr.ConsumeGoods(nameof(EconActivity.Food), foodConsumed);
    }

}



