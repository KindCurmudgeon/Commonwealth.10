using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;
using Commonwealth.Shared.EconomicMgrs;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Data;

public partial class VGame
{
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

            string? AddFoodStatusNews(FoodStatus foodStatus)
            {
                List<string> districts = [];
                foreach (VDistrict status in VDistricts)
                {
                    if (status.FoodMetrics?.FoodStatus == foodStatus)
                    {
                        districts.Add(status.Name);
                    }
                }
                if (districts.Count == 0) return null;
                return $"{Util.GetEnumString<FoodStatus>(foodStatus)} in {string.Join(", ", districts)}";
            }
        }
        void GameReports()
        {
            GameNews = new Report($"Game News");
            GameNews?.AddTextEntry($"Orders Due: {OrdersDueDate:yyyy.MM.dd HH:mm} GMT");
        }
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
 

    public GameStatus ExtractGameStatus()
    {
        return new GameStatus()
        {
            GameName = GameName,
            OrdersDueDate = OrdersDueDate,
            GameDate = GameDate,
            MarketDatas = MarketDatas,
            WorldNews = WorldNews,
            GameNews = GameNews,
            DistrictStatuses = ExtractDistrictStatus()
        };

        List<DistrictStatus> ExtractDistrictStatus()
        {
            List<DistrictStatus> districts = [];
            foreach (VDistrict vDistrict in VDistricts)
            {
                DistrictStatus district = new()
                {
                    Name = vDistrict.Name,
                    Population = vDistrict.Population,
                    DeltaPopulation = vDistrict.DeltaPopulation,
                    Owner = vDistrict.Owner,
                    Goods = vDistrict.Goods,
                    FoodMetrics = vDistrict.FoodMetrics,
                    StaffAvailable = vDistrict.StaffAvailable,
                    LastSeasonGoodsReport = vDistrict.LastSeasonGoodsReport,
                    LastSeasonVillageReport = vDistrict.LastSeasonVillageReport,
                    ForeignVillageCount = vDistrict.ForeignVillageCount
                };
                districts.Add(district);
            }
            return districts;
        }
    }
    public GameSetup ExtractGameSetup()
    {
        return new GameSetup()
        {
            Id = Id,
            GameName = GameName,
            HasPrescribedNationAssignments = HasPrescribedNationAssignments,
            IsDevelopmentGame = IsDevelopmentGame,
            GeogFileInfo = GeogFileInfo,
            InitFileInfo = InitFileInfo,
            EconFileInfo = EconFileInfo,
            GameState = GameState,
            Creator = Creator,
            Gamemasters = Gamemasters,
            OrdersPeriod = OrdersPeriod,
            ActivationDate = ActivationDate,
            EconParms = EconParms,
            Districts = ExtractDistrictSetups()
        };

        List<DistrictSetup> ExtractDistrictSetups()
        {
            List<DistrictSetup> districts = [];
            foreach (VDistrict vDistrict in VDistricts)
            {
                DistrictSetup district = new()
                {
                    Name = vDistrict.Name,
                    Resources = vDistrict.Resources,
                    AllowedVillages = vDistrict.AllowedVillages,
                    Region = vDistrict.Region,
                    Connections = vDistrict.Connections,
                    Features = vDistrict.Features
                };
                districts.Add(district);
            }
            return districts;
        }
    }

}
