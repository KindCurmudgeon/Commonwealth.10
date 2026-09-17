
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
    public GameSetup GameSetup {get;set;}
    public GameStatus GameStatus {get;set;}
    public WorldSetup WorldSetup {get;set;}
    public WorldStatus WorldStatus {get;set;}
    public List<Nation> Nations { get; set; }
    public List<NationNaming> NationNamings { get; set; }
    public List<string> WorldNewsItems { get; set; }
    [SetsRequiredMembers]
    public ServerEconomicMgr(GameSetup gameSetup, GameStatus gameStatus, WorldSetup worldSetup, WorldStatus worldStatus,  List<Nation> nations) : base(gameSetup.EconParms)
    {
        List<Village> allVillages = nations.GatherVillages();
        GameSetup = gameSetup;
        GameStatus = gameStatus;
        WorldSetup = worldSetup;
        WorldStatus = worldStatus;
        Nations = nations;
     //   EconParms = vGame.EconParms;
        AllNationMgrs = nations.AssembleNationMgrs();
        AllVillageMgrs = nations.AssembleVillageMgrs();
        AllDistrictMgrs = worldStatus.Districts.AssembleDistrictMgrs(worldSetup, allVillages);
        AllTradeMgrs = nations.AssembleTradeMgrs();
        AllSpyMgrs = nations.AssembleSpyMgrs();
        WorldNewsItems = [];
        MarketPrices = gameStatus.MarketDatas.AssembleMarketPrices();
        NationNamings = nations.AssembleNationNamings();
    }
}
