using System.Runtime.InteropServices;
using Commonwealth.Server.Data;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.MgrFactory;

public static class MgrPackageFactory
{
     public static MgrPackage Create(Game game, Nation nation, List<Nation> nations, List<District> ownedDistricts)
     {
          List<Village> villages = nations.AssembleVillages();//     public MgrPackage(Game game, Nation nation, List<Nation> nations, List<District> ownedDistricts)
          return new MgrPackage()
          {
               NationMgr = nation.CreateMgr(),
               VillageMgrs = nation.AssembleVillageMgrs(),
               DistrictMgrs = ownedDistricts.AssembleDistrictMgrs(villages),
               ExpansionDistrictMgrs = DistrictMgrFactory.AssembleExpansionDistrictMgrs(game.Districts, villages, nation.Identity.NationCode, nation.Trades),
               SpyMgrs = nation.AssembleSpyMgrs(),
               TradeMgrs = nation.AssembleTradeMgrs(),
          };
     }
}