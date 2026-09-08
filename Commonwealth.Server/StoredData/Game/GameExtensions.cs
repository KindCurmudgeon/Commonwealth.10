using System.Text.Json.Serialization;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Parameters;
using Commonwealth.Server.ServerEconomics;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;
using Commonwealth.Shared.EndpointDTOs;
using IdentityProvider.EndpointDTOs;

namespace Commonwealth.Server.Data;

public partial class Game
{
    public static Game Create(GameDTO dto, PlayerProfile creator, EconParms econParms, GeographyFile geogParmsFile, InitParms initParms)
    {
        Game game = new Game()
        {
            Id = Guid.NewGuid(),
            Name = Util.TitleCase(dto.GameName),
            GameState = GameState.Created,
            CreatorName = creator.UserName,
            CreationDate = DateTime.UtcNow,
            Gamemasters = [],
            OrdersPeriod = dto.OrdersPeriod ?? new TimeSpan(7, 0, 0, 0),
            GameDate = new GameDate(Util.GetRandomInclusive(initParms.InitialYearsStat ?? new Stat(600, 1300))),
            Districts = CreateDistricts(),
            EconParms = econParms,

            //   InitParms = initParms,

            GeogFileInfo = dto.GeogFileInfo ?? GeographyFile.DefaultFileInfo,
            InitFileInfo = dto.InitFileInfo,
            EconFileInfo = dto.EconFileInfo,
            WorldNews = new Report("World News - Last Season"),
            GameNews = new Report("Game News - Last Season")
        };
        CreateWorldNews();
        CreateGameNews();
        AssignResources();
        IdentifyAllowedVillages();
        InitializeMarket();
        return game;

        List<District> CreateDistricts()
        {
            List<District> districts = [];
            foreach (DistrictParm districtParm in geogParmsFile.Districts)
            {
                District district = District.Create(districtParm, geogParmsFile.Seas, econParms, initParms);
                //  district.CreateReport(game, null, null, )
                districts.Add(district);
            }
            return districts;
        }
        void AssignResources()
        {
            foreach (Resource resource in econParms.Resources)
            {
                List<District> elgible = game.Districts.Where(s => s.HasFeature(resource.Constraint)).ToList();
                Util.Shuffle(elgible);
                int netDistricts = (int)((resource.Existence ?? 1.0) * elgible.Count);
                while (netDistricts-- > 0)
                {
                    District district = elgible[0];
                    (district.Resources ??= []).Add(resource.Name);
                    elgible.RemoveAt(0);
                }
            }
            foreach (District district in game.Districts)
            {
                int shortfall = initParms.MinResourcePerDistrict - (district.Resources?.Count ?? 0);
                while (shortfall > 0)
                {
                    List<Resource> elgible = econParms.Resources.Where(r => district.HasFeature(r.Constraint)).ToList();
                    RemoveExisting(elgible);
                    string? resource = Util.PickRandomFromList<Resource>(elgible)?.Name;
                    if (resource is null) continue;
                    (district.Resources ??= []).Add(resource);
                    shortfall--;
                }

                void RemoveExisting(List<Resource> items)
                {
                    foreach (string resourceName in district.Resources ?? [])
                    {
                        items.RemoveAll(i => i.Name == resourceName);
                    }
                }
            }
        }

        void IdentifyAllowedVillages()  // SHould this go somewhere else after EconParms.VillageParms are created.
        {
            foreach (District district in game.Districts)
            {
                foreach (string resource in district.Resources ?? [])
                {
                    VillageParm? villageParm = econParms.VillageParms.Find(p => p.Resource == resource);
                    district.AllowedVillages ??= [];
                    district.AllowedVillages.AddIfNotNull(villageParm?.Name);
                }
                foreach (VillageParm tradingVillageParm in econParms.VillageParms.Where(p => p.Type == VILLAGETYPE.TRADING))
                {
                    Feature? constraint = tradingVillageParm.Constraint;
                    if (district.HasFeature(constraint))
                    {
                        district.AllowedVillages ??= [];
                        district.AllowedVillages?.Add(tradingVillageParm.Name);
                    }
                }
            }
        }
        void InitializeMarket()
        {
            game.MarketDatas = [];
            foreach (MarketInit init in initParms.InitialMarketGoods)
            {
                MarketData data = new MarketData(init);
                game.MarketDatas.Add(data);

            }
        }
        void CreateWorldNews()
        {
            game.AddFoodStatusNews(FoodStatus.RATIONING);
            game.AddFoodStatusNews(FoodStatus.FAMINE);
        }
        void CreateGameNews()
        {
            game.GameNews?.AddTextEntry($"Game '{game.Name}' created by '{game.CreatorName}'");
            game.GameNews?.AddTextEntry($"Created: {game.CreationDate: yyyy.MM.dd HH:mm} GMT.");
        }
    }
    public void Activate()
    {
        GameState = GameState.Activated;
        ActivationDate = DateTime.UtcNow;
        GameNews?.AddTextEntry($"Activated: {ActivationDate:yyyy.MM.dd HH:mm} GMT");
        OrdersDueDate = DateTime.UtcNow.Add(OrdersPeriod);
        GameNews?.AddTextEntry("-----------------");
        GameNews?.AddTextEntry($"Orders Due: {OrdersDueDate:yyyy.MM.dd HH:mm} GMT");
    }

    public void SeasonUpdate(List<String> worldNewsItems)
    {
        OrdersDueDate = DateTime.UtcNow.Add(OrdersPeriod);
        WorldReports();
        GameReports();

        void WorldReports()
        {
            WorldNews = new Report($"World News - Last Season");
            WorldNews.AddTextEntry(AddFoodStatusNews(FoodStatus.FAMINE));
            WorldNews.AddTextEntry(AddFoodStatusNews(FoodStatus.RATIONING));
            WorldNews.AddTitledList("Other News", worldNewsItems);

            string? AddFoodStatusNews(FoodStatus status)
            {
                List<string> districts = [];
                foreach (District district in Districts)
                {
                    if (district.FoodMetrics?.FoodStatus == status)
                    {
                        districts.Add(district.Name);
                    }
                }
                if (districts.Count == 0) return null;
                return $"{Util.GetEnumString<FoodStatus>(status)} in {string.Join(", ", districts)}";
            }
        }
        void GameReports()
        {
            GameNews = new Report($"Game News");
            GameNews?.AddTextEntry($"Orders Due: {OrdersDueDate:yyyy.MM.dd HH:mm} GMT");
        }
    }


    public District? FindDistrict(string name)
    {
        return Districts?.Find(d => d.Name == name);
    }
    public static async Task<bool> GameExists(string gameName, BlobService blobService)
    {
        return await blobService.IsExisting(Game.BlobPath(gameName));
    }

    public List<string> GatherAvailableHomes(List<Nation> nations)
    {
        List<string> available = Districts.Select(r => r.Name).ToList();
        List<string> taken = nations.Where(l => l.HomeDistrict is not null).Select(l => l.HomeDistrict!).ToList();
        foreach (string name in taken) available.Remove(name);
        return available;
    }
    public async Task<List<Nation>> GatherNationsAsync(BlobService blobService)
    {
        List<string> fileNames = await blobService.GetFileNamesWithPrefix(Nation.BlobPrefix(Name));
        List<string> fileErrors = [];
        List<Nation> nations = [];
        foreach (string fileName in fileNames)
        {
            try
            {
                Nation nation = await blobService.RetrieveAsync<Nation>(fileName);
                nations.Add(nation);
            }
            catch { fileErrors.Add($"{fileName} missing or corrupted"); }
        }
        foreach (Nation nation in nations)
        {
            if (GameDate.IsSame(nation.SeasonCount) == false)
            {
                fileErrors.Add($"GameDate mismatch for {nation.Naming.Name}");
            }
        }
        if (fileErrors.Count > 0)
        {
            string message = $"Errors found in nation files for game '{Name}': {string.Join("; ", fileErrors)}";
            throw new AppException(ExceptionType.Blob, BlobFailType.FileContent, message);
        }
        return nations;
    }
    public bool HasGamemasterAuthority(PlayerProfile player)
    {
        if (CreatorName == player.UserName) return true;
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
    public int GetWaitingCount(List<Nation> nations)
    {
        //  if (nations.Count == 0) return null;
        int count = 0;
        foreach (Nation nation in nations)
        {
            if (GameState == GameState.Created)
            {
                if (nation.LineupState != LineupState.Accepted) count++;
            }
            if (GameState == GameState.Activated)
            {
                if (nation.OrdersState != OrdersState.OrdersSubmitted) count++;
            }
        }
        return count;
    }

    public void AddFoodStatusNews(FoodStatus status)
    {
        List<string> districts = [];
        foreach (District district in Districts)
        {
            if (district.FoodMetrics?.FoodStatus == status)
            {
                districts.Add(district.Name);
            }
        }
        if (districts.Count == 0) return;
        string statusString = $"{Util.GetEnumString<FoodStatus>(status)} in {string.Join(", ", districts)}";
        WorldNews?.AddTextEntry(statusString);
    }
    public static async Task Remove(string gameName, BlobService blobService)
    {
        Game game = await Game.RetrieveAsync(gameName, blobService);
        await Remove(game, blobService);
    }
    public static async Task Remove(Game game, BlobService blobService)
    {
        List<Nation> nations = await game.GatherNationsAsync(blobService);

        List<BlobDescriptor> descriptors = [];
        foreach (Nation nation in nations)
        {
            try
            {
                Player player = await descriptors.RetrieveIfNotFoundAsync<Player>(Player.BlobPath(nation.PlayerName), blobService);
                player.RemoveAllGames(game.Name);
            }
            catch { }
        }
        foreach (string playerName in game.Gamemasters)
        {
            try
            {
                Player player = await descriptors.RetrieveIfNotFoundAsync<Player>(Player.BlobPath(playerName), blobService);
                player.RemoveAllGames(game.Name);
            }
            catch { }
        }
        try
        {
            Player creator = await descriptors.RetrieveIfNotFoundAsync<Player>(Player.BlobPath(game.CreatorName), blobService);
            creator.RemoveAllGames(game.Name);
        }
        catch { }
        await blobService.SaveGroupAsync(descriptors);
        await blobService.RemoveWithPrefix(Folders.Games, null, game.Name);

    }
}
public static class GameExtensions
{
    public static List<District> OwnedBy(this List<District> source, int? nationCode)
    {
        return source.Where(d => d.Owner == nationCode).ToList();
    }
    public static District? FindDistrict(this List<District> source, string? name)
    {
        if (name is null) return null;
        return source.Find(d => d.Name == name);
    }


}
public partial class GameDate
{
    [JsonInclude] public int SeasonCount { get; private set; }
    [JsonInclude] public int Year { get; private set; }
    [JsonConstructor] public GameDate() { }
    public override string ToString()
    {
        return $"Year: {Year}  (Season: {SeasonCount + 1})";
    }
    public GameDate(int year)
    {
        SeasonCount = -1;
        Year = year;
    }
    public void SeasonUpdate()
    {
        SeasonCount++;
        Year++;
    }

    public bool IsSame(int? other)
    {
        if (other is null) return false;
        return SeasonCount == other;
    }
}
