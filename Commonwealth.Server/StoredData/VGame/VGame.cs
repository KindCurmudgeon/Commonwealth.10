using System.Diagnostics.CodeAnalysis;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Data;

public partial class VGame : GameAuthority
{
     public Guid Id { get; set; } // Both
     public required string GameName { get; set; } // Both
     public bool? HasPrescribedNationAssignments { get; set; } = true;  // Setup
     public bool? IsDevelopmentGame { get; set; } = true; // Setup
     public ParmFileInfo? GeogFileInfo { get; set; } // Setup
     public ParmFileInfo? InitFileInfo { get; set; } // Setup
     public ParmFileInfo? EconFileInfo { get; set; } // Setup
     public GameState GameState { get; set; } // Setup
                                              //   public required string CreatorName { get; set; } // Setup
                                              //  public List<string> Gamemasters { get; set; } = []; // Setup
     public TimeSpan OrdersPeriod { get; set; } // Setup
     public List<VDistrict> VDistricts { get; set; } = []; // Setup
     public DateTime? ActivationDate { get; set; } // Setup
     public EconParms EconParms { get; set; } = default!; // Setup


     public DateTime? OrdersDueDate { get; set; } // Seasonal
     public GameDate GameDate { get; set; } = default!; // Seasonal
     public List<MarketData>? MarketDatas { get; set; } // Seasonal
                                                        //  public required InitParms InitParms { get; set; }
     public Report? WorldNews { get; set; } // Seasonal
     public Report? GameNews { get; set; } // Seasonal

     [SetsRequiredMembers]
     public VGame(GameSetup setup, GameStatus status)
     {
          Id = setup.Id;
          GameName = setup.GameName;
          HasPrescribedNationAssignments = setup.HasPrescribedNationAssignments;
          IsDevelopmentGame = setup.IsDevelopmentGame;
          GeogFileInfo = setup.GeogFileInfo;
          InitFileInfo = setup.InitFileInfo;
          EconFileInfo = setup.EconFileInfo;
          GameState = setup.GameState;
          Creator = setup.Creator;
          Gamemasters = setup.Gamemasters;
          OrdersPeriod = setup.OrdersPeriod;
          ActivationDate = setup.ActivationDate;
          EconParms = setup.EconParms;

          OrdersDueDate = status.OrdersDueDate;
          GameDate = status.GameDate;
          MarketDatas = status.MarketDatas;
          WorldNews = status.WorldNews;
          GameNews = status.GameNews;

          VDistricts = CreateVDistricts(setup, status);
     }
     public static async Task<VGame> Load(string gameName, BlobService blobService)
     {
          GameSetup gameSetup = await GameSetup.RetrieveAsync(gameName, blobService);
          GameStatus gameStatus = await GameStatus.RetrieveAsync(gameName, blobService);
          return new VGame(gameSetup, gameStatus);
     }
     private static List<VDistrict> CreateVDistricts(GameSetup gameSetup, GameStatus gameStatus)
     {
          List<VDistrict> vDistricts = [];
          foreach (DistrictStatus districtStatus in gameStatus.DistrictStatuses)
          {
               DistrictSetup? districtSetup = gameSetup.FindDistrict(districtStatus.Name);
               if (districtSetup is not null) vDistricts.Add(new VDistrict(districtSetup, districtStatus));
          }
          return vDistricts;
     }
     public VDistrict? FindDistrict(string districtName)
     {
          return VDistricts.Find(d => d.Name == districtName);
     }
     public List<string> GatherAvailableHomes(List<Nation> nations)
     {
          List<string> available = VDistricts.Select(r => r.Name).ToList();
          List<string> taken = nations.Where(l => l.HomeDistrict is not null).Select(l => l.HomeDistrict!).ToList();
          foreach (string name in taken) available.Remove(name);
          return available;
     }
     public async Task<List<Nation>> GatherNationsConfirmDatesAsync(BlobService blobService)
     {
          List<string> fileNames = await blobService.GetFileNamesWithPrefix(Nation.BlobPrefix(GameName));
          List<string> fileErrors = [];
          List<Nation> nations = [];
          foreach (string fileName in fileNames)
          {
               try
               {
                    Nation nation = await blobService.RetrieveAsync<Nation>(fileName);
                    nations.Add(nation);
               }
               catch { fileErrors.Add($"Error retrieving {fileName}."); }

               foreach (Nation nation in nations)
               {
                    if (this.GetType().GetProperty("GameDate") != null)
                    {
                         if (GameDate.IsSame(nation.SeasonCount) == false)
                         {
                              fileErrors.Add($"GameDate mismatch for {nation.Naming.Name}");
                         }
                    }
               }
          }
          if (fileErrors.Count > 0)
          {
               string message = $"Errors found in nation files for game '{GameName}': {string.Join("; ", fileErrors)}";
               throw new AppException(ExceptionType.Blob, BlobFailType.FileContent, message);
          }
          return nations;
     }

}
public static class VGameExpansion
{
     public static List<Village> GatherVillages(this List<Nation> nations)
     {
          List<Village> villages = [];
          foreach (Nation nation in nations)
          {
               villages.AddRange(nation.Villages ?? []);
          }
          return villages;
     }
     public static void ConfirmSameSeason(this Nation nation, VGame vGame)
     {
          if ((vGame.GameDate?.IsSame(nation.SeasonCount) ?? false) == false)
               throw new AppException(ExceptionType.Endpoint, EndpointFailType.SeasonUpdated, $"{nation.Naming.Name}");
     }
}

public interface IGameAuthority
{
     public List<string> Gamemasters { get; set; }
     public string Creator { get; set; }
}
public class GameAuthority
{
     public List<string> Gamemasters { get; set; } = [];
     public required string Creator { get; set; }

     public bool HasGamemasterAuthority(PlayerProfile player)
     {
          if (Creator == player.UserName) return true;
          if (Gamemasters.Exists(g => g == player.UserName)) return true;
          if (player.RoleLevel >= RoleLevel.Admin) return true;
          return false;
     }
     public void ConfirmGamemasterAuthority(PlayerProfile player)
     {
          if (HasGamemasterAuthority(player) is false)
               throw new AppException(ExceptionType.Auth, AuthFailType.UserNotAuthorized, player.UserName);
     }
     public void ConfirmGameVisibilityAuthority(PlayerProfile player, List<Nation> nations)
     {
          foreach (Nation nation in nations)
          {
               if (nation.PlayerName == player.UserName) return;
          }
          ConfirmGamemasterAuthority(player);
     }

}
