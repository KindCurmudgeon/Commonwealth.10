
using System.Diagnostics.CodeAnalysis;
using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints;
using Commonwealth.Server.MgrFactory;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.ServerEconomics;

public partial class ServerEconomicMgr : EconomicMgr
{
    public VGame VGame { get; set; }
    public List<Nation> Nations { get; set; }
    public List<NationNaming> NationNamings { get; set; }
    public List<string> WorldNewsItems { get; set; }
    [SetsRequiredMembers]
    public ServerEconomicMgr(VGame vGame, List<Nation> nations) : base(vGame.EconParms)
    {
        List<Village> allVillages = nations.GatherVillages();
        VGame = vGame;
        Nations = nations;
        EconParms = vGame.EconParms;
        AllNationMgrs = nations.AssembleNationMgrs();
        AllVillageMgrs = nations.AssembleVillageMgrs();
        AllDistrictMgrs = vGame.VDistricts.AssembleDistrictMgrs(allVillages);
        AllTradeMgrs = nations.AssembleTradeMgrs();
        AllSpyMgrs = nations.AssembleSpyMgrs();
        WorldNewsItems = [];
        MarketPrices = VGame.MarketDatas.AssembleMarketPrices();
        NationNamings = nations.AssembleNationNamings();
    }
}
