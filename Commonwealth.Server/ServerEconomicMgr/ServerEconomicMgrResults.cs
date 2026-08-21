

using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Server.ServerEconomics;

public partial class ServerEconomicMgr
{
    public void DetermineResults()
    {
        AdjustMarketTradesAsNeeded();
        HandleTradeSends();
        HandleTradeReceives();
        foreach (DistrictMgr dMgr in AllDistrictMgrs) HandleFoodConsumption(dMgr);
        foreach (DistrictMgr dMgr in AllDistrictMgrs) HandleVillagesHere(dMgr);
        HandleSpies();

        void HandleTradeSends()
        {
            foreach (TradeMgr tradeMgr in AllTradeMgrs)
            {
                NationMgr? nationMgr = AllNationMgrs.Find(n => n.Identity.NationCode == tradeMgr.Order.SendingNationCode);
                if (nationMgr is null) continue;
                HandleTradeSend(tradeMgr, nationMgr);
            }
        }
        void HandleTradeReceives()
        {
            foreach (TradeMgr tradeMgr in AllTradeMgrs)
            {
                NationMgr? nationMgr = AllNationMgrs.Find(n => n.Identity.NationCode == tradeMgr.Order.SendingNationCode);
                if (nationMgr is null) continue;
                HandleTradeReceive(tradeMgr, nationMgr);
            }
        }
        void HandleSpies()
        {
            foreach (SpyMgr spyMgr in AllSpyMgrs)
            {
                NationMgr? nationMgr = AllNationMgrs.Find(n => n.Identity.NationCode == spyMgr.NationCode);
                if (nationMgr is null) continue;
                HandleSpy(spyMgr, nationMgr);
            }
        }
        void HandleVillagesHere(DistrictMgr districtMgr)
        {
            List<VillageMgr> priority = GetPriorties();
            foreach (VillageMgr mgr in priority) VillageActivation(mgr, districtMgr, GetNationMgr(mgr));
            foreach (VillageMgr mgr in priority) VillageOperation(mgr, districtMgr, GetNationMgr(mgr));
            foreach (VillageMgr mgr in priority) VillageConstruction(mgr, districtMgr, GetNationMgr(mgr));
            foreach (VillageMgr mgr in priority) VillageProduction(mgr, GetNationMgr(mgr));

            NationMgr GetNationMgr(VillageMgr vMgr) => AllNationMgrs.Find(nm => nm.NationCode == vMgr.Identity.Owner)!;

            List<VillageMgr> GetPriorties()
            {
                List<VillageMgr> villagesHere = AllVillageMgrs.Here(districtMgr.Name);
                priority = villagesHere.OwnedBy(districtMgr.Owner ?? 0);
                priority = priority.OrderByDescending(vm => vm.Order.PremiumWages).ToList();
                List<VillageMgr> remaining = villagesHere.Except(priority).ToList();
                remaining = remaining.OrderByDescending(vm => vm.Order.PremiumWages).ToList();
                priority.AddRange(remaining);
                return priority;
            }

        }
        void AdjustMarketTradesAsNeeded()
        {
            List<TradeMgr> marketTrades = AllTradeMgrs.Where(t => t.Order?.TradeType == TRADETYPE.MARKET).ToList();
            List<Asset> netRequested = GatherMarketTradeRequests();
            foreach (Asset requestedAsset in netRequested)
            {
                MarketData? Available = Game.MarketDatas?.Find(m => m.Name == requestedAsset.Name);
                int threshhold = (int)((Available?.Inventory ?? 0) * (EconParms.MarketParms.MaxMarketBuyPercent ?? 1));
                if (requestedAsset.Amount < threshhold) continue;
                double reductionFactor = (double)threshhold / requestedAsset.Amount;
                foreach (TradeMgr mgr in marketTrades)
                {
                    TradeOrder order = mgr.Order!;
                    int newAmount = (int)Math.Round(order.Receive!.Amount * reductionFactor);
                    order.Receive.Amount = newAmount;
                }
            }

            List<Asset> GatherMarketTradeRequests()
            {
                List<Asset> netRequested = [];
                foreach (TradeMgr mgr in marketTrades)
                {
                    TradeOrder? order = mgr.Order;
                    if (order?.Send is null || order.Receive is null) continue;
                    //   if (SetMarketTradeReceive(order) is false) continue;
                    Asset receiveAsset = order.Receive;
                    Asset? found = netRequested.Find(a => a.Name == order.Receive.Name);
                    if (found == null)
                    {
                        netRequested.AddIfNotNull(order.Receive?.DeepCopy());
                    }
                    else
                    {
                        found.Amount += order.Receive.Amount;
                    }
                }
                return netRequested;
            }
        }
    }

}