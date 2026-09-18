using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static class InfoPackageFactory
{
    public static InfoPackage Create(
 GameSetup gameSetup,
 GameStatus gamestatus,
        List<DistrictStatus> ownedDistricts,
        WorldSetup worldSetup,
        List<Nation> nations,
        int nationCode,
        MgrPackage mgrPackage)
    {
        EconParms econParms = gameSetup.EconParms;
        List<string> connections = worldSetup.GatherConnections(ownedDistricts);
        return new InfoPackage()
        {
            GameName = gameSetup.GameName,
            SeasonString = gamestatus.GameDate.ToString(),
            EconParms = econParms,
            NationGoodNames = econParms.GoodParms.Select(p => p.Name).ToList(),
            DistrictGoodNames = econParms.GoodParms.Where(p => p.Type != GOODTYPE.CURRENCY).Select(g => g.Name).ToList(),
            VillageNames = econParms.VillageParms.Select(v => v.Name).ToList(),
            NationNamings = nations.Select(n => n.Naming).ToList(),
            MarketPrices = gamestatus.MarketDatas.AssembleMarketPrices(),
            WorldNews = gamestatus.WorldNews,
            GameNews = gamestatus.GameNews,
            SpyTargets = CreateSpyTargets(connections),
            DistrictTradeTargets = CreateDistrictTradeTargets(mgrPackage.DistrictMgrs, connections),
            NationTradeTargets = CreateNationTradeTargets(nations, nationCode)
        };

        // List<string> GatherConnections(List<District> ownedDistricts)
        // {
        //     List<string> connections = [];
        //     foreach (District district in ownedDistricts)
        //     {
        //         foreach (string connection in district.Connections)
        //         {
        //             string? match = ownedDistricts.FindDistrict(connection)?.Name;
        //             if (match is not null) connections.AddIfNotDuplicate(connection);
        //         }
        //     }
        //     return connections;
        // }

        List<string?> CreateDistrictTradeTargets(List<DistrictMgr> ownedDistricts, List<string> ownedDistrictConnections)
        {
            List<string?> tradeTargets = ownedDistricts.Select(n => n.Name).Cast<string?>().ToList();
            tradeTargets.Add(null);
            tradeTargets.AddRange(ownedDistrictConnections);
            return tradeTargets;
        }
        List<string> CreateSpyTargets(List<string> ownedDistrictConnections)
        {
            return new List<string>(ownedDistrictConnections);
        }
        List<string>? CreateNationTradeTargets(List<Nation> nations, int myNationCode)
        {
            List<string> targets = [];
            foreach (Nation nation in nations)
            {
                if (nation.Identity.NationCode == myNationCode) continue;
                targets.Add(nation.Naming.Name ?? "unknown");
            }
            return (targets.Count <= 1) ? null : targets;

        }
    }
}