using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static class InfoPackageFactory
{
    public static InfoPackage Create(Game game, Nation nation, List<Nation> nations, MgrPackage mgrPackage)
    {

        List<District> ownedDistricts = game.Districts.OwnedBy(mgrPackage.NationMgr.Identity.NationCode);
        List<string> ownedDistrictConnections = GatherConnections(ownedDistricts);
        EconParms EconParms = game.EconParms;
        return new InfoPackage()
        {
            GameName = game.Name,
            SeasonString = game.GameDate.ToString(),
            EconParms = EconParms,
            NationGoodNames = EconParms.GoodParms.Select(p => p.Name).ToList(),
            DistrictGoodNames = EconParms.GoodParms.Where(p => p.Type != GOODTYPE.CURRENCY).Select(g => g.Name).ToList(),
            VillageNames = EconParms.VillageParms.Select(v => v.Name).ToList(),
            NationNamings = nations.Select(n => n.Naming).ToList(),
            MarketPrices = game.MarketDatas.AssembleMarketPrices(),
            WorldNews = game.WorldNews,
            GameNews = game.GameNews,
            OwnedDistrictConnections = GatherConnections(ownedDistricts),
            SpyTargets = CreateSpyTargets(ownedDistrictConnections),
            DistrictTradeTargets = CreateDistrictTradeTargets(mgrPackage.DistrictMgrs, ownedDistrictConnections),
            NationTradeTargets = CreateNationTradeTargets(nations, nation.Identity.NationCode)
        };

        List<string> GatherConnections(List<District> ownedDistricts)
        {
            List<string> connections = [];
            foreach (District district in ownedDistricts)
            {
                foreach (string connection in district.Connections)
                {
                    string? match = ownedDistricts.FindDistrict(connection)?.Name;
                    if (match is not null) connections.AddIfNotDuplicate(connection);
                }
            }
            return connections;
        }

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