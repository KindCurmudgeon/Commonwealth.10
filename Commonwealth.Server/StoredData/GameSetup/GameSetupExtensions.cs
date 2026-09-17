using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Parameters;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Data;

public partial class GameSetup : GameAuthority
{
    public static GameSetup Create(GameDTO dto, PlayerProfile creator, EconParms econParms, GeographyFile geogParmsFile, InitParms initParms)
    {
        GameSetup setup = new GameSetup()
        {
            Id = Guid.NewGuid(),
            GameName = Util.TitleCase(dto.GameName),
            GameState = GameState.Created,
            Creator = creator.UserName,
            CreationDate = DateTime.UtcNow,
            Gamemasters = [],
            OrdersPeriod = dto.OrdersPeriod ?? new TimeSpan(7, 0, 0, 0),
  //          Districts = CreateDistricts(),
            EconParms = econParms,
            GeogFileInfo = dto.GeogFileInfo ?? GeographyFile.DefaultFileInfo,
            InitFileInfo = dto.InitFileInfo,
            EconFileInfo = dto.EconFileInfo,
            //   WorldNews = new Report("World News - Last Season"),
            //  GameNews = new Report("Game News - Last Season")
        };

     //   AssignResources();
     //   IdentifyAllowedVillages();
        // InitializeMarket();
        return setup;


        // void InitializeMarket()
        // {
        //     setup.MarketDatas = [];
        //     foreach (MarketInit init in initParms.InitialMarketGoods)
        //     {
        //         MarketData data = new MarketData(init);
        //         setup.MarketDatas.Add(data);

        //     }
        // }
        // void CreateWorldNews()
        // {
        //     setup.AddFoodStatusNews(FoodStatus.RATIONING);
        //     setup.AddFoodStatusNews(FoodStatus.FAMINE);
        // }
        // void CreateGameNews()
        // {
        //     setup.GameNews?.AddTextEntry($"Game '{setup.Name}' created by '{setup.CreatorName}'");
        //     setup.GameNews?.AddTextEntry($"Created: {setup.CreationDate: yyyy.MM.dd HH:mm} GMT.");
        // }
    }
    

    public static async Task Remove(GameSetup gameSetup, BlobService blobService)
    {
        await gameSetup.RemoveGameFromPlayers(blobService);
        await blobService.RemoveWithPrefix(Folders.Games, gameSetup.GameName, gameSetup.GameName);
    }
    public async Task RemoveGameFromPlayers(BlobService blobService)
    {
        List<BlobDescriptor> descriptors = [];

        List<Nation> nations = await GatherNationsAsync(blobService);
        List<string> playerNames = nations.Select(n => n.PlayerName).ToList();
        playerNames.AddIfNotDuplicate(Creator);
        foreach (string gm in Gamemasters) playerNames.AddIfNotDuplicate(gm);
        foreach (string playerName in playerNames)
        {
            Player player = await Player.RetrieveAsync(playerName, blobService);
            player.RemoveAllGames(GameName);
            descriptors.Add(player.BlobDescriptor());
        }
        await blobService.SaveGroupAsync(descriptors);
    }

    public async Task<List<Nation>> GatherNationsAsync(BlobService blobService)
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
        }
        if (fileErrors.Count > 0)
        {
            string message = $"Errors found in nation files for game '{GameName}': {string.Join("; ", fileErrors)}";
            throw new AppException(ExceptionType.Blob, BlobFailType.FileContent, message);
        }
        return nations;
    }

 

    // public void Activate()
    // {
    //     GameState = GameState.Activated;
    //     ActivationDate = DateTime.UtcNow;
    //     GameNews?.AddTextEntry($"Activated: {ActivationDate:yyyy.MM.dd HH:mm} GMT");
    //     OrdersDueDate = DateTime.UtcNow.Add(OrdersPeriod);
    //     GameNews?.AddTextEntry("-----------------");
    //     GameNews?.AddTextEntry($"Orders Due: {OrdersDueDate:yyyy.MM.dd HH:mm} GMT");
    // }

    // public void SeasonUpdate(List<String> worldNewsItems)
    // {
    //     OrdersDueDate = DateTime.UtcNow.Add(OrdersPeriod);
    //     WorldReports();
    //     GameReports();

    //     void WorldReports()
    //     {
    //         WorldNews = new Report($"World News - Last Season");
    //         WorldNews.AddTextEntry(AddFoodStatusNews(FoodStatus.FAMINE));
    //         WorldNews.AddTextEntry(AddFoodStatusNews(FoodStatus.RATIONING));
    //         WorldNews.AddTitledList("Other News", worldNewsItems);

    //         string? AddFoodStatusNews(FoodStatus status)
    //         {
    //             List<string> districts = [];
    //             foreach (District district in Districts)
    //             {
    //                 if (district.FoodMetrics?.FoodStatus == status)
    //                 {
    //                     districts.Add(district.Name);
    //                 }
    //             }
    //             if (districts.Count == 0) return null;
    //             return $"{Util.GetEnumString<FoodStatus>(status)} in {string.Join(", ", districts)}";
    //         }
    //     }
    //     void GameReports()
    //     {
    //         GameNews = new Report($"Game News");
    //         GameNews?.AddTextEntry($"Orders Due: {OrdersDueDate:yyyy.MM.dd HH:mm} GMT");
    //     }
    // }


    // public DistrictSetup? FindDistrictSetup(string name)
    // {
    //     return DistrictSetups?.Find(d => d.Name == name);
    // }
    public static async Task<bool> GameExists(string gameName, BlobService blobService)
    {
        return await blobService.IsExisting(GameSetup.BlobPath(gameName));
    }


    // public async Task<List<Nation>> GatherNationsAsync(BlobService blobService)
    // {
    //     List<string> fileNames = await blobService.GetFileNamesWithPrefix(Nation.BlobPrefix(Name));
    //     List<string> fileErrors = [];
    //     List<Nation> nations = [];
    //     foreach (string fileName in fileNames)
    //     {
    //         try
    //         {
    //             Nation nation = await blobService.RetrieveAsync<Nation>(fileName);
    //             nations.Add(nation);
    //         }
    //         catch { fileErrors.Add($"{fileName} missing or corrupted"); }
    //     }
    //     foreach (Nation nation in nations)
    //     {
    //         if (GameDate.IsSame(nation.SeasonCount) == false)
    //         {
    //             fileErrors.Add($"GameDate mismatch for {nation.Naming.Name}");
    //         }
    //     }
    //     if (fileErrors.Count > 0)
    //     {
    //         string message = $"Errors found in nation files for game '{Name}': {string.Join("; ", fileErrors)}";
    //         throw new AppException(ExceptionType.Blob, BlobFailType.FileContent, message);
    //     }
    //     return nations;
    // }



    // public void AddFoodStatusNews(FoodStatus status)
    // {
    //     List<string> districts = [];
    //     foreach (District district in Districts)
    //     {
    //         if (district.FoodMetrics?.FoodStatus == status)
    //         {
    //             districts.Add(district.Name);
    //         }
    //     }
    //     if (districts.Count == 0) return;
    //     string statusString = $"{Util.GetEnumString<FoodStatus>(status)} in {string.Join(", ", districts)}";
    //     WorldNews?.AddTextEntry(statusString);
    // }
    // public static async Task Remove(string gameName, BlobService blobService)
    // {
    //     Game game = await Game.RetrieveAsync(gameName, blobService);
    //     await Remove(game, blobService);
    // }
    // public static async Task Remove(Game game, BlobService blobService)
    // {
    //     List<Nation> nations = await game.GatherNationsAsync(blobService);

    //     List<BlobDescriptor> descriptors = [];
    //     foreach (Nation nation in nations)
    //     {
    //         try
    //         {
    //             Player player = await descriptors.RetrieveIfNotFoundAsync<Player>(Player.BlobPath(nation.PlayerName), blobService);
    //             player.RemoveAllGames(game.Name);
    //         }
    //         catch { }
    //     }
    //     foreach (string playerName in game.Gamemasters)
    //     {
    //         try
    //         {
    //             Player player = await descriptors.RetrieveIfNotFoundAsync<Player>(Player.BlobPath(playerName), blobService);
    //             player.RemoveAllGames(game.Name);
    //         }
    //         catch { }
    //     }
    //     try
    //     {
    //         Player creator = await descriptors.RetrieveIfNotFoundAsync<Player>(Player.BlobPath(game.CreatorName), blobService);
    //         creator.RemoveAllGames(game.Name);
    //     }
    //     catch { }
    //     await blobService.SaveGroupAsync(descriptors);
    //     await blobService.RemoveWithPrefix(Folders.Games, null, game.Name);

}

// public static class GameExtensions
// {
//     public static List<District> OwnedBy(this List<District> source, int? nationCode)
//     {
//         return source.Where(d => d.Owner == nationCode).ToList();
//     }
//     public static District? FindDistrict(this List<District> source, string? name)
//     {
//         if (name is null) return null;
//         return source.Find(d => d.Name == name);
//     }


// }
// public partial class GameDate
// {
//     [JsonInclude] public int SeasonCount { get; private set; }
//     [JsonInclude] public int Year { get; private set; }
//     [JsonConstructor] public GameDate() { }
//     public override string ToString()
//     {
//         return $"Year: {Year}  (Season: {SeasonCount + 1})";
//     }
//     public GameDate(int year)
//     {
//         SeasonCount = -1;
//         Year = year;
//     }
//     public void SeasonUpdate()
//     {
//         SeasonCount++;
//         Year++;
//     }

//     public bool IsSame(int? other)
//     {
//         if (other is null) return false;
//         return SeasonCount == other;
//     }
// }
