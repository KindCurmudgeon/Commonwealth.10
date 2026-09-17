using System.Diagnostics.CodeAnalysis;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Parameters;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Data;

// public partial class VGame : GameSetup
// {
//      public GameState GameState {get;set;}
//      public DateTime? OrdersDueDate { get; set; } // Seasonal
//      public GameDate GameDate { get; set; } = default!; // Seasonal
//      public List<MarketData>? MarketDatas { get; set; } // Seasonal
//      public List<VDistrict> VDistricts {get;set;}

//                                                         //  public required InitParms InitParms { get; set; }
//      public Report? WorldNews { get; set; } // Seasonal
//      public Report? GameNews { get; set; } // Seasonal

//      [SetsRequiredMembers]
//      public VGame(GameSetup setup, GameStatus status)
//      {
//           Id = setup.Id;
//           GameName = setup.GameName;
//           HasPrescribedNationAssignments = setup.HasPrescribedNationAssignments;
//           IsDevelopmentGame = setup.IsDevelopmentGame;
//           GeogFileInfo = setup.GeogFileInfo;
//           InitFileInfo = setup.InitFileInfo;
//           EconFileInfo = setup.EconFileInfo;
 
//           Creator = setup.Creator;
//           Gamemasters = setup.Gamemasters;
//           OrdersPeriod = setup.OrdersPeriod;
//           ActivationDate = setup.ActivationDate;
//           EconParms = setup.EconParms;

//          GameState = status.GameState;
//           OrdersDueDate = status.OrdersDueDate;
//           GameDate = status.GameDate;
//           MarketDatas = status.MarketDatas;
//           WorldNews = status.WorldNews;
//           GameNews = status.GameNews;
//           VDistricts = CreateVDistricts(setup, status);
//      }
//      public static async Task<VGame> Load(string gameName, BlobService blobService)
//      {
//           GameSetup gameSetup = await RetrieveAsync(gameName, blobService);
//           GameStatus? gameStatus = await GameStatus.RetrieveIfExistsAsync(gameName, blobService);
//           gameStatus ??= GameStatus.Default(gameName);
//           return new VGame(gameSetup, gameStatus);
//      }
//      private static List<VDistrict> CreateVDistricts(GameSetup gameSetup, GameStatus gameStatus)
//      {
//           List<VDistrict> vDistricts = [];
//           foreach (DistrictSetup districtSetup in gameSetup.Districts)
//           {
//                DistrictStatus districtStatus= gameStatus.DistrictStatuses.First(s=>s.Name == districtSetup.Name);
//                vDistricts.Add(new VDistrict(districtSetup, districtStatus));
//           }
//           return vDistricts;
//      }
//      public List<string> GatherAvailableHomes(List<Nation> nations)
//      {
//           List<string> available = VDistricts.Select(r => r.Name).ToList();
//           List<string> taken = nations.Where(l => l.HomeDistrict is not null).Select(l => l.HomeDistrict!).ToList();
//           foreach (string name in taken) available.Remove(name);
//           return available;
//      }
//      public async Task<List<Nation>> GatherNationsConfirmDatesAsync(BlobService blobService)
//      {
//           List<string> fileNames = await blobService.GetFileNamesWithPrefix(Nation.BlobPrefix(GameName));
//           List<string> fileErrors = [];
//           List<Nation> nations = [];
//           foreach (string fileName in fileNames)
//           {
//                try
//                {
//                     Nation nation = await blobService.RetrieveAsync<Nation>(fileName);
//                     nations.Add(nation);
//                }
//                catch { fileErrors.Add($"Error retrieving {fileName}."); }

//                foreach (Nation nation in nations)
//                {
//                     if (this.GetType().GetProperty("GameDate") != null)
//                     {
//                          if (GameDate.IsSame(nation.SeasonCount) == false)
//                          {
//                               fileErrors.Add($"GameDate mismatch for {nation.Naming.Name}");
//                          }
//                     }
//                }
//           }
//           if (fileErrors.Count > 0)
//           {
//                string message = $"Errors found in nation files for game '{GameName}': {string.Join("; ", fileErrors)}";
//                throw new AppException(ExceptionType.Blob, BlobFailType.FileContent, message);
//           }
//           return nations;
//      }

// }
// public static class VGameExpansion
// {
//      public static List<Village> GatherVillages(this List<Nation> nations)
//      {
//           List<Village> villages = [];
//           foreach (Nation nation in nations)
//           {
//                villages.AddRange(nation.Villages ?? []);
//           }
//           return villages;
//      }
//      public static void ConfirmSameSeason(this Nation nation, VGame vGame)
//      {
//           if ((vGame.GameDate?.IsSame(nation.SeasonCount) ?? false) == false)
//                throw new AppException(ExceptionType.Endpoint, EndpointFailType.SeasonUpdated, $"{nation.Naming.Name}");
//      }
// }



