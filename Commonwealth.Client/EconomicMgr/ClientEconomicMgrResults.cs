
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Client.ClientEconomics;
public partial class ClientEconomicMgr
{
    public void DetermineResults()
    {
        ResetEconomics();
        HandleTradeSends();
        HandleTradeReceives();
        foreach (DistrictMgr districtMgr in AllDistrictMgrs) HandleFoodConsumption(districtMgr);
        foreach (DistrictMgr districtMgr in AllDistrictMgrs) HandleVillagesHere(districtMgr);
        HandleSpies();
    }
    private void ResetEconomics()
    {
        NationMgr.Reset();
        foreach (DistrictMgr mgr in AllDistrictMgrs) mgr.Reset();
    }
    private void HandleVillagesHere(DistrictMgr districtMgr)
    {
        List<VillageMgr> vMgrHere = AllVillageMgrs.Here(districtMgr.Name).OrderByDescending(vm => vm.Order.PremiumWages).ToList();
        foreach (VillageMgr mgr in vMgrHere) VillageActivation(mgr, districtMgr, NationMgr);
        foreach (VillageMgr mgr in vMgrHere) VillageOperation(mgr, districtMgr, NationMgr);
        foreach (VillageMgr mgr in vMgrHere) VillageConstruction(mgr, districtMgr, NationMgr);
        foreach (VillageMgr mgr in vMgrHere) VillageProduction(mgr, NationMgr);
    }
    private void HandleTradeSends()
    {
        foreach (TradeMgr mgr in AllTradeMgrs)
        {
            HandleTradeSend(mgr, NationMgr);
        }
    }
    private void HandleTradeReceives()
    {
        foreach (TradeMgr mgr in AllTradeMgrs)
        {
            HandleTradeReceive(mgr, NationMgr);
        }
    }
    private void HandleSpies()
    {
        foreach (SpyMgr mgr in AllSpyMgrs)
        {
            HandleSpy(mgr, NationMgr);
        }
    }
}