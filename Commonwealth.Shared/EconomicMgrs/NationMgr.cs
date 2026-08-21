using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Shared.EconomicMgrs;


public partial class NationMgr : EconActivity, IEconActivity
{
    public required NationIdentity Identity { get; init; }
    public NationNaming? Naming { get; set; }
    public OrdersState OrdersState { get; set; }
    public int? SeasonCount { get; set; }
    public required List<string> Adjustments { get; set; } = [];
    public EconActivity? LastSeasonEconActivity { get; set; }
    public List<string>? LastSeasonAdjustments { get; set; }
    public int NationCode { get => Identity.NationCode; }
    public int? Population { get; set; }
    public Report? LastSeasonReport { get; set; }
    [JsonConstructor] public NationMgr(){}
    [SetsRequiredMembers] public NationMgr(NationIdentity identity, List<Asset>? goodsAvailable, int? staffAvailable) 
                        : base(goodsAvailable, staffAvailable) { Identity = identity;}
    public void Reset() { ResetEconActivity(); }

    public Report CreateReport(string? subTitle, List<DistrictMgr> ownedDistrictMgrs, List<string>? goodNames)
    {
        Report report = new Report($"{Naming?.FormalNation} - Goods {subTitle}");
        string districtList = string.Join(", ", ownedDistrictMgrs.Select(d => d.Name));
        string districtHeader = "District".Pluralize(ownedDistrictMgrs.Count);
        report.AddTextEntry($"{districtHeader}: {districtList}");
        report.AddTextEntry($"Population: {Population}");
        report.AddTitledList("Adjustments", Adjustments);
        ReportTableData tableData = new ReportTableData("Activity")
        {
            FirstHeading = "",
            ColumnHeadings = goodNames ?? [],
            RowDatas = CreateTableRows(goodNames)
        };
        report.AddTable(tableData);
        return report;
    }
}

