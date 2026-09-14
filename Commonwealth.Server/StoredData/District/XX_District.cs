using System.Text.Json.Serialization;
using Commonwealth.Server.Parameters;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Server.Data;

// public partial class DistrictSetup : DistrictGeography
// {
//     public int Population { get; set; } // Season
//     public int DeltaPopulation { get; set; }// Season
//     public int Owner { get; set; }// Season
//     public List<Asset>? Goods { get; set; }// Season
//     public List<string>? Resources { get; set; } // Setup
//     public List<string>? AllowedVillages { get; set; } //Setup
//     public FoodMetrics? FoodMetrics { get; set; } // Season
//     public int StaffAvailable { get; set; } // Season
//     public Report? LastSeasonGoodsReport { get; set; } // Season
//     public Report? LastSeasonVillageReport { get; set; } // Season
//     public int? ForeignVillageCount { get; set; } // Season
//     [JsonConstructor] public District() : base() { }

//     public static DistrictSetup Create(DistrictParm geog, List<DistrictParm>? seas, EconParms econParms, InitParms initParms)
//     {
//         List<string> connections = geog.LandConnections ?? [];
//         AddSeaConnections(geog, seas, 2);
//         District district = new District()
//         {
//             Name = geog.Name,
//       //      Possessive = geog.Possessive,
//             Region = geog.Region ?? "Sea",
//             Connections = connections,
//             Population = Util.GetRandomInclusive(initParms.InitialDistrictPopulationStat),
//             Goods = GoodStat.GetRandomGoodStat(initParms.InitialDistrictGoods),
//             Owner = 0,
//             Features = Util.StringToEnumList<Feature>(geog.Features)
//         };
//         district.UpdateFoodMetrics(econParms);
//         switch (district.FoodMetrics?.FoodStatus)
//         {
//             case FoodStatus.RATIONING: district.StaffAvailable = (int)(district.Population * 0.5); break;
//             case FoodStatus.FAMINE: district.StaffAvailable = 0; break;
//             default: district.StaffAvailable = district.Population; break;
//         }

//         return district;

//         void AddSeaConnections(DistrictParm sourceParm, List<DistrictParm>? seas, int depthLevel)
//         {
//             List<string> seasChecked = [];

//             foreach (string first in sourceParm?.SeaConnections ?? [])
//             {
//                 ProcessSeaLayer(first, depthLevel);
//             }
//             void ProcessSeaLayer(string seaName, int depth)
//             {
//                 if (depth <= 0) return;
//                 seasChecked.Add(seaName);
//                 DistrictParm? seaParm = seas?.Find(s => s.Name == seaName);
//                 foreach (string connection in seaParm?.LandConnections ?? [])
//                 {
//                     connections!.AddIfNotDuplicate(connection);
//                 }
//                 foreach (string connection in seaParm?.SeaConnections ?? [])
//                 {
//                     if (connection == sourceParm?.Name) continue;
//                     if (seasChecked.Contains(connection)) continue;
//                     ProcessSeaLayer(connection, depth - 1);
//                 }
//             }
//         }
//     }

// }
// // public partial class DistrictGeography
// // {
// //     public required string Name { get; set; }
// //    // public required string Possessive { get; set; }
// //     public required string Region { get; set; }
// //     public required List<string> Connections { get; set; }
// //     public List<Feature>? Features { get; set; }
// //     [JsonConstructor] public DistrictGeography() { }

// // }

