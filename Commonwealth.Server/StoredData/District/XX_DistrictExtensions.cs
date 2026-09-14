using System.Resources;
using System.Text;
using Commonwealth.Server.Parameters;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Data;

// public partial class District
// {

//     public void Activate(int owner)
//     {
//         Owner = owner;
//     }

//     public void SeasonUpdate(DistrictMgr? districtMgr, 
//                 List<VillageMgr> villageMgrsHere, 
//                 Report? WorldNews, 
//                 List<NationNaming> namings, 
//                 EconParms econParms)
//     {
//         if (districtMgr is null) return;
//         DeltaPopulation = (districtMgr.Population ?? 0) - Population;
//         Population = districtMgr.Population ?? 0;
//         Goods = districtMgr.Final;
//         UpdateFoodMetrics(econParms);
//         UpdateOwnership();
//         List<string> districtGoods = econParms.GoodParms.Where(p=>p.Type != GOODTYPE.CURRENCY).Select(g=>g.Name).ToList();
//         LastSeasonGoodsReport = districtMgr.CreateGoodsReport("Last Season", districtGoods);
//         LastSeasonVillageReport = districtMgr.CreateVillageReport("Last Season");
//         LastSeasonVillageReport.AddTitledList("Foreign Villages", GatherForeignVillageHere());

//         List<string> GatherForeignVillageHere()
//         {
//             List<string> foreignVillages = [];
//             List<VillageMgr> foreignVillagesHere = villageMgrsHere.Where(v=>v.Identity.Owner != districtMgr.Owner).ToList();
//             foreach (VillageMgr villageMgr in foreignVillagesHere)
//             {
//                 foreignVillages.Add(villageMgr.Summary(namings));
//             }
//             return foreignVillages;
//         }
//         void UpdateOwnership()
//         {
//             List<CensusItem> census = GetVillageCensus();
//             int maxVillageCount = 0;
//             foreach (CensusItem item in census) { if (item.ActiveVillageCount > maxVillageCount) maxVillageCount = item.ActiveVillageCount; }
//             if (maxVillageCount < econParms.MinVillagesNeededForDistrictOwnership) return;
//             int countAtMax = 0;
//             int winningNationCode = 0;
//             foreach (CensusItem item in census)
//             {
//                 if (item.ActiveVillageCount == maxVillageCount)
//                 {
//                     countAtMax++;
//                     winningNationCode = item.NationCode;
//                 }
//             }
//             if (countAtMax >= 2) return;  // A tie!
//             if (winningNationCode != 0)
//             {
//                 Owner = winningNationCode;
//                 WorldNews?.AddTextEntry($"{Name} is now owned by {namings.NameOf(winningNationCode)}");
//             }

//             List<CensusItem> GetVillageCensus()
//             {
//                 List<CensusItem> censuses = [];
//                 foreach (VillageMgr mgr in villageMgrsHere)
//                 {
//                     CensusItem? census = censuses.Find(d => d.NationCode == mgr.Identity.Owner);
//                     if (census is null) censuses.Add(new CensusItem(mgr.Identity.Owner, mgr.Active));
//                     else census.ActiveVillageCount += mgr.Active;
//                 }
//                 return censuses;
//             }
//         }
//     }
//     public void UpdateFoodMetrics(EconParms? econParms)
//     {
//         if (econParms is null || Goods is null || Population == 0) return;
//         int nominalFoodConsumed = (int)Math.Floor(Population * econParms.FoodConsumptionPerCapita);
//         List<Asset>? foodAssets = Goods?.Where(g => econParms.FoodTypes.Contains(g.Name)).ToList();
//         int foodAvailableAmount = foodAssets?.Sum(a => a.Amount) ?? 0;
//         double seasonsOfFood = (nominalFoodConsumed == 0) ? 99999 : Math.Round((double)foodAvailableAmount / nominalFoodConsumed, 1);
//         FoodStatus foodStatus = GetFoodStatus();
//         int actualFoodConsumed = (foodStatus != FoodStatus.OK) ?
//               (int)Math.Floor(seasonsOfFood / econParms.FoodRationingThreshhold * nominalFoodConsumed) :
//               nominalFoodConsumed;
//         StringBuilder foodString = new();
//         foodString.Append("Food (Avail/Needed/Seasons): ");
//         foodString.Append($"{foodAvailableAmount} / {actualFoodConsumed} / {seasonsOfFood}");
//         if (foodStatus != FoodStatus.OK) foodString.Append($" - {Util.GetEnumString<FoodStatus>(foodStatus)}");
//         FoodMetrics = new FoodMetrics()
//         {
//             FoodNeeded = nominalFoodConsumed,
//             FoodConsumed = actualFoodConsumed,
//             AllocatedFoodConsumed = GetAllocatedFoodConsumption(foodAssets, actualFoodConsumed),
//             SeasonsOfFood = seasonsOfFood,
//             ProductionCapacity = econParms.ProductionCapabilityCurve?.Lookup(seasonsOfFood) ?? 1.0,
//             FoodStatus = foodStatus,
//             Summary = foodString.ToString()
//         };

//         FoodStatus GetFoodStatus()
//         {
//             if (seasonsOfFood < 1.0) return FoodStatus.FAMINE;
//             if (seasonsOfFood < econParms.FoodRationingThreshhold) return FoodStatus.RATIONING;
//             return FoodStatus.OK;
//         }


//         List<Asset>? GetAllocatedFoodConsumption(List<Asset>? foodAvail, int foodNeededAmount)
//         {
//             if (foodAvail is null) return null;
//             int foodAvailableAmount = foodAvail.Sum(a => a.Amount);
//             double foodRatio = (double)foodNeededAmount / foodAvailableAmount;
//             List<Asset> allocated = [];
//             int net = 0;
//             foreach (Asset asset in foodAvail)
//             {
//                 int amount = (int)Math.Round(foodRatio * asset.Amount, MidpointRounding.AwayFromZero);
//                 net += amount;
//                 allocated.Add(new Asset(asset.Name, amount));
//             }
//             if (allocated.Count == 0) return allocated;
//             int adjustedIndex = IndexOfMax(allocated);
//             int neededAdjustment = foodNeededAmount - net;
//             allocated[adjustedIndex].Amount += neededAdjustment;
//             return allocated;

//             int IndexOfMax(List<Asset> assets)
//             {
//                 int maxIndex = 0;
//                 int maxValue = 0;
//                 int n = 0;
//                 foreach (Asset asset in assets)
//                 {
//                     if (asset.Amount > maxValue)
//                     {
//                         maxValue = asset.Amount;
//                         maxIndex = n;
//                     }
//                     n++;
//                 }
//                 return maxIndex;
//             }
//         }
//     }

//     private class CensusItem(int nationCode, int activeVillageCount)
//     {
//         public int NationCode { get; set; } = nationCode;
//         public int ActiveVillageCount { get; set; } = activeVillageCount;
//         public bool IsMax { get; set; } = false;
//     }

//     // public void CreateReport(DistrictMgr mgr, GameDate gameDate, List<Village>? VillagesHere, List<NationNaming> namings, List<VillageParm> villageParms)
//     // {
//     //     Report report = new Report($"{Name} - Last Season");
//     //     report.AddItem($"Owner: {namings.NameOf(Owner)}");
//     //     report.AddItem($"Population: {Population} ({Util.SignedWithPlus(DeltaPopulation)})");
//     //     report.AddItem($"Goods Available: {Goods?.AssetString()}");
//     //     report.AddItem(FoodMetrics?.Summary ?? "");
//     //     report.AddTitledList("<b><u>Economic Activity</u></b>:", mgr.SummarizeEcon());
//     //     report.AddTitledList("<b><u>Villages</u></b>:", GatherVillageSummaries());
//     //     report.AddItem($"Unemployed: {CreateEmploymentString()}");
//     //     Report = report;
//     //     List<string> GatherVillageSummaries()
//     //     {
//     //         List<string> summaries = [];
//     //         foreach (Village village in VillagesHere?.Where(v => v.Identity.Owner == Owner).ToList() ?? [])
//     //         {
//     //             NationNaming? naming = namings.Find(n => n.NationCode == village.Identity.Owner);
//     //             string item = $"{naming?.Possessive} {village.Identity.Name} village".Pluralize(village.Count);
//     //             summaries.Add(item);
//     //         }
//     //         List<Village> otherVillages = VillagesHere?.Where(v => v.Identity.Owner != Owner).ToList() ?? [];
//     //         otherVillages.GroupBy(v => v.Identity.Owner).ToList();
//     //         ForeignVillageCount = 0;
//     //         foreach (Village village in otherVillages)
//     //         {
//     //             NationNaming? naming = namings.Find(n => n.NationCode == village.Identity.Owner);
//     //             string item = $"{naming?.Possessive} {village.Identity.Name} village".Pluralize(village.Count);
//     //             summaries.Add(item);
//     //             ForeignVillageCount++;
//     //         }
//     //         return summaries;
//     //     }
//     //     string CreateEmploymentString()
//     //     {
//     //         int workers = 0;
//     //         foreach (Village village in VillagesHere ?? [])
//     //         {
//     //             VillageParm? parm = villageParms.Find(v => v.Name == village.Identity.Name);
//     //             workers += (parm?.Workers ?? 0) * village.Active;
//     //         }
//     //         int unemployed = Population - workers;
//     //         int percent = (int)((double)unemployed / Population) * 100;
//     //         return $"{unemployed} ({percent}%)";
//     //     }
//     // }

//     public bool HasFeature(Feature? feature)
//     {
//         if (feature is null || feature == Feature.NONE) return true;
//         Feature? found = Features?.Find(f => f == feature);
//         return found is not null;
//     }
//     public bool IsVillageAllowedHere(string? villageType)
//     {
//         if (villageType is null) return false;
//         return AllowedVillages?.Contains(villageType) ?? false;
//     }
// }




