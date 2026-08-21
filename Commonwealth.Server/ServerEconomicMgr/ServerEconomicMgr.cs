
using System.Diagnostics.CodeAnalysis;
using Commonwealth.Server.Data;
using Commonwealth.Server.MgrFactory;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.ServerEconomics;

public partial class ServerEconomicMgr : EconomicMgr
{
    public Game Game { get; set; }
    public List<Nation> Nations { get; set; }
    public List<NationNaming> NationNamings { get; set; }
    public List<string> WorldNewsItems { get; set; }
    [SetsRequiredMembers] public ServerEconomicMgr(Game game, List<Nation> nations) : base(game.EconParms)
    {
        List<Village> allVillages = nations.AssembleVillages();
        Game = game;
        Nations = nations;
        EconParms = game.EconParms;
        AllNationMgrs = nations.AssembleNationMgrs();
        AllVillageMgrs = nations.AssembleVillageMgrs();
        AllDistrictMgrs = game.Districts.AssembleDistrictMgrs(allVillages);
        AllTradeMgrs = nations.AssembleTradeMgrs();
        AllSpyMgrs = nations.AssembleSpyMgrs();
        WorldNewsItems = [];
        MarketPrices = Game.MarketDatas.AssembleMarketPrices();
        NationNamings = nations.AssembleNationNamings();
    }

}
