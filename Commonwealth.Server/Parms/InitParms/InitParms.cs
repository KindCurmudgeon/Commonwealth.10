using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EndpointDTOs;


namespace Commonwealth.Server.Parameters;

public class InitParms
{
    public required List<MarketInit> InitialMarketGoods { get; set; }
    public required List<GoodStat> InitialDistrictGoods { get; set; }
    public required List<GoodStat> InitialNationGoods { get; set; }
    public required Stat InitialDistrictPopulationStat { get; set; }
    public required Stat InitialYearsStat { get; set; }
    // public required Stat InitialGoodsFactorStat { get; set; }
    public required int MinResourcePerDistrict { get; set; }


    [JsonConstructor] public InitParms() { }

    [SetsRequiredMembers]
    public InitParms(InitParmsFile? file)
    {
        InitialMarketGoods = file?.InitialMarketGoods ?? Defaults.InitialMarketGoods;
        InitialDistrictGoods = file?.InitialDistrictGoods ?? Defaults.InitialDistrictGoods;
        InitialDistrictPopulationStat = file?.InitialDistrictPopulationStat ?? Defaults.InitDistrictPopulationStat;
        InitialYearsStat = file?.InitialYearsStat ?? Defaults.InitialYearsStat;
        //   InitialGoodsFactorStat = file?.InitialGoodsFactorStat ?? Defaults.InitialGoodsFactorStat;
        InitialNationGoods = file?.InitialNationGoods ?? Defaults.InitialNationGoods;
        MinResourcePerDistrict = file?.MinResourcePerDistrict ?? Defaults.MinResourcePerDistrict;
    }
    private static class Defaults
    {
        public static List<MarketInit> InitialMarketGoods = new()
        {
            new(GOODS.GRAIN,10000,10000),
            new(GOODS.FISH,10000,10000),
            new(GOODS.WOOD,10000,10000),
            new(GOODS.BRICK,10000,10000),
        };
        public static List<GoodStat> InitialDistrictGoods = new()
        {
            new(GOODS.GRAIN,new Stat(70,130)),
            new(GOODS.FISH,new Stat(70,130))
        };
        public static List<GoodStat> InitialNationGoods = new()
        {
            new(GOODS.GRAIN, new Stat(70,130)),
            new(GOODS.FISH, new Stat(70,130)),
            new(GOODS.WOOD, new Stat(50,150)),
            new(GOODS.BRICK, new Stat(50,150)),
            new(GOODS.GEMS,new Stat(1000,3000))
        };
        public static Stat InitDistrictPopulationStat = new(3000, 9000);
        public static Stat InitialYearsStat = new(300, 1300);
        public static Stat InitialGoodsFactorStat = new(70, 130);
        public static int MinResourcePerDistrict = 2;

    }
}
public partial class InitParmsFile : IBlobObject
{
    public required ParmFileInfo ParmFileInfo { get; set; }
    public List<MarketInit>? InitialMarketGoods { get; set; }
    public List<GoodStat>? InitialDistrictGoods { get; set; }
    public List<GoodStat>? InitialNationGoods { get; set; }
    public Stat? InitialDistrictPopulationStat { get; set; }
    public Stat? InitialYearsStat { get; set; }
    //   public Stat? InitialGoodsFactorStat { get; set; }
    public int? MinResourcePerDistrict { get; set; }

    [JsonConstructor] public InitParmsFile() { }
}

public class GoodStat
{
    public string? Name { get; set; }
    public Stat? Stat { get; set; }
    [JsonConstructor] public GoodStat() { }
    public GoodStat(string name, Stat stat)
    {
        Name = name;
        Stat = stat;
    }
    public static List<Asset>? GetRandomGoodStat(List<GoodStat> source)
    {
        List<Asset> result = [];
        foreach (GoodStat goodStat in source)
        {
            if (goodStat.Name is null || goodStat.Stat is null) continue;
            result.Add(new Asset(goodStat.Name, Util.GetRandomInclusive(goodStat.Stat)));
        }
        return (result.Count == 0) ? null : result;
    }
}