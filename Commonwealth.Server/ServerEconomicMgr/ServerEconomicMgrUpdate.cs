
using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Server.ServerEconomics;

public partial class ServerEconomicMgr
{
    public void UpdateForNextSeason()
    {
        UpdateMarket();
        UpdateTrades();
        UpdateVillages();
        UpdateDistricts();  // Villages before Districts due to potential Owner changes
        UpdateSpies();
        UpdateNations();
        GameStatus.SeasonUpdate(WorldNewsItems, GameSetup, WorldStatus);
        AdvanceDates();

        void AdvanceDates()
        {
            int newSeasonCount = GameStatus.GameDate.SeasonUpdate();
            foreach (Nation nation in Nations) nation.SeasonCount = newSeasonCount;
        }

        void UpdateMarket()
        {
            foreach (MarketData marketData in GameStatus.MarketDatas ?? [])
            {
                marketData.SeasonUpdate(AllTradeMgrs, GameStatus.MarketDatas ?? []);
            }
            List<TradeMgr> marketTrades = AllTradeMgrs.Where(t => t.Order?.TradeType == TRADETYPE.MARKET).ToList();
        }
        void UpdateTrades()
        {
            foreach (TradeMgr tradMgr in AllTradeMgrs)
            {
                Nation? nation = Nations.Find(n => n.Identity.NationCode == tradMgr.Order.SendingNationCode);
                if (tradMgr.Order.IsStandingTrade is false)
                {
                    nation?.Trades?.RemoveAll(t => t.Id == tradMgr.Id);
                }
            }
        }

        void UpdateVillages()
        {
            foreach (VillageMgr villageMgr in AllVillageMgrs)
            {
                Nation? nation = Nations.Find(n => n.Identity.NationCode == villageMgr.Identity.Owner);
                Village? village = nation?.Villages?.Find(v => v.Identity.Equals(villageMgr.Identity));
                // if (village is null) village = new Village(villageMgr.Order, villageMgr.Count, villageMgr.Active);
                village?.SeasonUpdate(villageMgr);
            }
        }

        void UpdateSpies()
        {
            foreach (SpyMgr mgr in AllSpyMgrs)
            {
                Nation? nation = Nations.Find(n => n.Identity.NationCode == mgr.NationCode);
                if (nation is null) continue;
                if (mgr.Order.Status == SpyState.ToBeRetired)
                {
                    nation.Spies?.RemoveAll(s => s.Id == mgr.Id);
                }
                if (mgr.Order.Status == SpyState.Added)
                {
                    nation.Spies?.Add(new Spy(mgr));
                }

                Spy? spy = nation?.Spies?.Find(s => s.Id == mgr.Id);
                spy?.SeasonUpdate(mgr, WorldStatus, nation);
            }
        }
        void UpdateDistricts()
        {
            foreach (DistrictStatus district in WorldStatus.Districts)
            {
                DistrictMgr? mgr = AllDistrictMgrs.Find(d => d.Name == district.Name);
                district.SeasonUpdate(mgr, AllVillageMgrs.Here(district.Name), GameStatus.WorldNews, NationNamings, EconParms);
                //  district?.CreateReport(Game.GameDate, Nations);
            }
        }

        void UpdateNations()
        {
            foreach (NationMgr nationMgr in AllNationMgrs)
            {
                Nation? nation = Nations.Find(n => n.Identity.NationCode == nationMgr.Identity.NationCode);
                List<DistrictMgr> myDistricts = AllDistrictMgrs.Where(d => d.Owner == nationMgr.Identity.NationCode).ToList();
                nation?.SeasonUpdate(nationMgr, myDistricts, EconParms.GoodParms.Select(p => p.Name).ToList());
            }
        }


        // void DistrictReports()
        // {
        //     // foreach (DistrictMgr mgr in AllDistrictMgrs)
        //     // {
        //     //     District? district = Game.Districts.Find(d => d.Name == mgr.Name);
        //     //     if (district is null) continue;
        //     //     List<Village> villagesHere = district.Villages ?? [];
        //     //     district.CreateReport(mgr, Game.GameDate, villagesHere, NationNamings, EconParms.VillageParms);
        //     // }
        // }

        // void DistrictReports()
        // {
        //     List<Village> allVillages = CollectAllVillages();
        //     foreach (District district in Game.Districts)
        //     {
        //         List<VillageMgr> VillagesHere = AllVillageMgrs.Here(district.Name);
        //         DistrictMgr? mgr = DistrictMgrs.Find(d => d.Name == district.Name);
        //         if (mgr is null) continue;
        //         List<Village> villagesHere = allVillages.Where(v => v.Identity.District == district.Name).ToList();
        //         district.CreateReport(mgr, Game.GameDate, villagesHere, NationNamings, EconParms.VillageParms);
        //     }
        //     List<Village> CollectAllVillages()
        //     {
        //         List<Village> villages = [];
        //         foreach (Nation nation in Nations)
        //         {
        //             if (nation.Villages is null) continue;
        //             villages.AddRange(nation.Villages);
        //         }
        //         return villages;
        //     }
        // }
        // void NationReports()
        // {
        //     foreach (Nation nation in Nations)
        //     {
        //         //  List<District> myDistricts = Game.Districts.Where(d => d.Owner == nation.Identity.NationCode).ToList();
        //         NationMgr? mgr = NationMgrs.Find(n => n.Identity.NationCode == nation.Identity.NationCode);
        //         if (mgr is null) continue;
        //         nation.CreateReport(mgr, Game.GameDate);
        //     }
        // }
        // void SpyReports()
        // {
        //     foreach (Nation nation in Nations)
        //     {
        //         nation.Spies?.ForEach(s => s.CreateReport(Game.Districts));
        //     }
        // }


    }
}

//     public void SeasonUpdate(List<String> worldNewsItems)
//     {
//         OrdersDueDate = DateTime.UtcNow.Add(OrdersPeriod);
//         WorldReports();
//         GameReports();

//         void WorldReports()
//         {
//             WorldNews = new Report($"World News - Last Season");
//             WorldNews.AddTextEntry(AddFoodStatusNews(FoodStatus.FAMINE));
//             WorldNews.AddTextEntry(AddFoodStatusNews(FoodStatus.RATIONING));
//             WorldNews.AddTitledList("Other News", worldNewsItems);

//             string? AddFoodStatusNews(FoodStatus foodStatus)
//             {
//                 List<string> districts = [];
//                 foreach (VDistrict status in VDistricts)
//                 {
//                     if (status.FoodMetrics?.FoodStatus == foodStatus)
//                     {
//                         districts.Add(status.Name);
//                     }
//                 }
//                 if (districts.Count == 0) return null;
//                 return $"{Util.GetEnumString<FoodStatus>(foodStatus)} in {string.Join(", ", districts)}";
//             }
//         }
//         void GameReports()
//         {
//             GameNews = new Report($"Game News");
//             GameNews?.AddTextEntry($"Orders Due: {OrdersDueDate:yyyy.MM.dd HH:mm} GMT");
//         }
//     }