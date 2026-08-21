

using System.Diagnostics.CodeAnalysis;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Client.ClientEconomics;

public partial class ClientEconomicMgr : EconomicMgr
{
    public InfoPackage InfoPackage { get; set; } = default!;
 
    public NationMgr NationMgr { get; set; } = default!;


    [SetsRequiredMembers]
    public ClientEconomicMgr(MgrPackage mgrPackage, InfoPackage infoPackage)

    {
        InfoPackage = infoPackage;
        EconParms = infoPackage.EconParms;
        NationMgr = mgrPackage.NationMgr;
        AllNationMgrs = new List<NationMgr>() { NationMgr };
        AllDistrictMgrs = mgrPackage.DistrictMgrs;
        AllDistrictMgrs.AddRange(mgrPackage.ExpansionDistrictMgrs);
        AllVillageMgrs = mgrPackage.VillageMgrs;
        AllTradeMgrs = mgrPackage.TradeMgrs;
        AllSpyMgrs = mgrPackage.SpyMgrs;
        isClient = OperatingSystem.IsBrowser();
        MarketPrices = infoPackage.MarketPrices ?? [];
    }
}
