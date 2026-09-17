using Commonwealth.Server.Data;
using Commonwealth.Server.MgrFactory;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class OrdersEndpoints
{
    public static async Task GetOrdersAsync(Nation nation, BlobService blobService, OrdersResponse response)
    {
        string gameName = nation.Identity.GameName;
        int nationCode = nation.Identity.NationCode;
        GameSetup gameSetup = await GameSetup.RetrieveAsync(gameName, blobService);
        GameStatus gameStatus = await GameStatus.RetrieveAsync(gameName, blobService);
        gameStatus.ConfirmSameSeason(nation);
        List<Nation> nations = await gameStatus.GatherNationsConfirmDatesAsync(blobService);
        List<Village> villages = nations.GatherVillages();
        WorldStatus worldStatus = await WorldStatus.RetrieveAsync(gameName,blobService);
        WorldSetup worldSetup = await WorldSetup.RetrieveAsync(gameName, blobService);
        List<DistrictStatus> ownedDistricts =  worldStatus.GetOwnedDistricts(nationCode);

        response.MgrPackage = MgrPackageFactory.Create(nation, ownedDistricts, villages, worldSetup);
        response.InfoPackage = InfoPackageFactory.Create(gameSetup, gameStatus, ownedDistricts, worldSetup, nations, nationCode, response.MgrPackage);

        // List<MarketPrice> GatherMarketPrices()
        // {
        //     List<MarketPrice> data = [];
        //     foreach (MarketData good in game.MarketDatas ?? [])
        //     {
        //         data.Add(new MarketPrice(good));
        //     }
        //     return data;
        // }

        // List<Village> GatherVillages(List<Nation> nations)
        // {
        //     List<Village> villages = [];
        //     foreach (Nation nation in nations)
        //     {
        //         villages.AddRange(nation.Villages ?? []);
        //     }
        //     return villages;
        // }

        // List<string?> GatherExpansionTargets()
        // {
        //     List<string?> targets = GatherConnections();
        //     return targets;
        // }

        // List<string?> GatherSpyTargets()
        // {
        //     List<string?> targets = GatherConnections();
        //     foreach (Spy spy in nation.Spies ?? [])
        //     {
        //         targets.RemoveAll(t => t == spy.District);
        //     }
        //     return targets;
        // }
        // List<string?> GatherTradeTargets()
        // {
        //     List<string?> targets = ownedDistricts.Select(d => d.Name).ToList().Cast<string?>().ToList();
        //     targets.Add(null);
        //     List<string?> others = GatherConnections();
        //     targets.AddRange(others);
        //     // foreach (string? target in others)
        //     // {
        //     //     targets.AddIfNotDuplicate(target);
        //     // }
        //     foreach (Trade trade in nation.Trades ?? [])
        //     {
        //         if (trade.Order.TradeType != TRADETYPE.DISTRICT) continue;
        //         targets.RemoveAll(t => t == trade.Order.DistrictName);
        //     }
        //     return targets;
        // }
        // List<string> GatherConnections()
        // {
        //     List<string> connections = [];
        //     foreach (District district in ownedDistricts)
        //     {
        //         foreach (string connection in district.Connections)
        //         {
        //             string? match = ownedDistricts.FindDistrict(connection)?.Name;
        //             if (match is null) connections.AddIfNotDuplicate(connection);
        //         }
        //     }
        //     return connections;
        // }
    }

}
// public partial class MarketPrice
// {
//     [SetsRequiredMembers]
//     public MarketPrice(MarketData data)
//     {
//         Name = data.Name;
//         Price = data.Price;
//     }
// }
// public partial class InfoPackage
// {
//     public InfoPackage(Game game, Nation nation, List<Nation> nations, MgrPackage mgrPackage)
//     {
//         // MyNationIdentity = nation.Identity;
//         // MyNationCode = MyNationIdentity.NationCode;
//         GameName = game.Name;
//         SeasonString = game.GameDate.ToString();
//         EconParms = game.EconParms;
//         NationGoodNames = EconParms.GoodParms.Select(p => p.Name).ToList();
//         DistrictGoodNames = EconParms.GoodParms.Where(p => p.Type != GOODTYPE.CURRENCY).Select(g => g.Name).ToList();
//         VillageNames = EconParms.VillageParms.Select(v => v.Name).ToList();
//         NationNamings = nations.Select(n => n.Naming).ToList();
//         MarketPrices = game.MarketDatas.AssembleMarketPrices();
//         WorldNews = game.WorldNews;
//         GameNews = game.GameNews;

//         List<District> ownedDistricts = game.Districts.OwnedBy(mgrPackage.NationMgr.Identity.NationCode);
//         OwnedDistrictConnections = GatherConnections(ownedDistricts);
//         UpdateExpansionTargets(mgrPackage.DistrictMgrs);
//         CreateSpyTargets();
//         CreateDistrictTradeTargets(mgrPackage.DistrictMgrs);
//         CreateNationTradeTargets(nations, nation.Identity.NationCode);

//         static List<string> GatherConnections(List<District> ownedDistricts)
//         {
//             List<string> connections = [];
//             foreach (District district in ownedDistricts)
//             {
//                 foreach (string connection in district.Connections)
//                 {
//                     string? match = ownedDistricts.FindDistrict(connection)?.Name;
//                     if (match is not null) connections.AddIfNotDuplicate(connection);
//                 }
//             }
//             return connections;
//         }

//         void CreateDistrictTradeTargets(List<DistrictMgr> ownedDistricts)
//         {
//             List<string?> tradeTargets = ownedDistricts.Select(n => n.Name).Cast<string?>().ToList();
//             tradeTargets.Add(null);
//             tradeTargets.AddRange(OwnedDistrictConnections ?? []);
//             DistrictTradeTargets = tradeTargets;
//         }
//         void CreateSpyTargets()
//         {
//             SpyTargets = new(OwnedDistrictConnections ?? []);
//         }
//         void CreateNationTradeTargets(List<Nation> nations, int myNationCode)
//         {
//             List<string> targets = [];
//             foreach (Nation nation in nations)
//             {
//                 if (nation.Identity.NationCode == myNationCode) continue;
//                 targets.Add(nation.Naming.Name ?? "unknown");
//             }
//             NationTradeTargets = (targets.Count <= 1) ? null : targets;

//         }
//     }
// }
// public partial class MgrPackage
// {
//     public MgrPackage(Game game, Nation nation, List<Nation> nations, List<District> ownedDistricts)
//     {
//         NationMgr = new NationMgr(nation);
//         List<Village> villages = nations.AssembleVillages();
//         VillageMgrs = nation.AssembleVillageMgrs();
//         DistrictMgrs = ownedDistricts.AssembleDistrictMgrs(villages);
//         ExpansionDistrictMgrs = nation.AssembleExpansionDistrictMgrs(game.Districts);
//         SpyMgrs = nation.AssembleSpyMgrs();
//         TradeMgrs = nation.AssembleTradeMgrs();
//     }
// }
