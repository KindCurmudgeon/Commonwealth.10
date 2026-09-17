using System.Diagnostics.CodeAnalysis;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;

namespace Commonwealth.Server.Data;

// public partial class VDistrict : DistrictSetup // Virtual District
// {
//      public int Population { get; set; } // Season
//      public int DeltaPopulation { get; set; }// Season
//      public int Owner { get; set; }// Season
//      public List<Asset>? Goods { get; set; }// Season
//   //   public List<string>? Resources { get; set; } // Setup
//   //   public List<string>? AllowedVillages { get; set; } //Setup
//      public FoodMetrics? FoodMetrics { get; set; } // Season
//      public int StaffAvailable { get; set; } // Season
//      public Report? LastSeasonGoodsReport { get; set; } // Season
//      public Report? LastSeasonVillageReport { get; set; } // Season
//      public int? ForeignVillageCount { get; set; } // Season

//      [SetsRequiredMembers]
//      public VDistrict(DistrictSetup districtSetup, DistrictStatus districtStatus) : base()
//      {
//           Name = districtSetup.Name;
//           Region = districtSetup.Region;
//           Connections = districtSetup.Connections;
//           Features = districtSetup.Features;
//           Resources = districtSetup.Resources;
//           AllowedVillages = districtSetup.AllowedVillages;

//           Population = districtStatus.Population;
//           DeltaPopulation = districtStatus.DeltaPopulation;
//           Owner = districtStatus.Owner;
//           Goods = districtStatus.Goods;
//           FoodMetrics = districtStatus.FoodMetrics;
//           StaffAvailable = districtStatus.StaffAvailable;
//           LastSeasonGoodsReport = districtStatus.LastSeasonGoodsReport;
//           LastSeasonVillageReport = districtStatus.LastSeasonVillageReport;
//           ForeignVillageCount = districtStatus.ForeignVillageCount;
//      }

// }

// public static class DistrictFactory
// {


//      public static List<VDistrict> GatherOwnedDistricts(this VGame vGame, int nationCode)
//      {
//           return vGame.VDistricts.Where(d => d.Owner == nationCode).ToList();
//      }

//      public static List<string> GatherConnections(this List<VDistrict> districts)
//      {
//           List<string> connections = [];
//           foreach (VDistrict district in districts)
//           {
//                foreach (string connection in district.Connections)
//                {
//                     string? match = district.Connections.Find(c => c == connection);
//                     if (match is not null) connections.AddIfNotDuplicate(connection);
//                }
//           }
//           return connections;
//      }
//      public static List<string> GatherConnections(this VGame vGame, List<VDistrict> ownedDistricts)
//      {
//           List<string> connections = [];
//           foreach (VDistrict district in ownedDistricts)
//           {
//                foreach (string connection in district.Connections)
//                {
//                     string? match = vGame.FindVDistrict(connection)?.Name;
//                     if (match is not null) connections.AddIfNotDuplicate(connection);
//                }
//           }
//           return connections;
//      }

//      // public static DistrictSetup FindDistrict(this GameSetup gameSetup, string districtName)
//      // {
//      //      return gameSetup.DistrictSetups.First(s => s.Name == districtName);
//      // }
//      // public static DistrictStatus FindDistrict(this GameStatus gameStatus, string districtName)
//      // {
//      //      return gameStatus.DistrictStatuses.First(s => s.Name == districtName);
//      // }
// }