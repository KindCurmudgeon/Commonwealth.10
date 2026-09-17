using System.Runtime.CompilerServices;
using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Parameters;
using Commonwealth.Server.ServerEconomics;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;


namespace Commonwealth.Server.Endpoints;


public static partial class GamemasterEndpoints
{
    public static async Task ActivateAsync(
        string gameName,
        PlayerProfile requestor,
        bool prescribedNations,
        BlobService blobService,
        ResponseBase response
    )
    {
        GameSetup gameSetup = await GameSetup.RetrieveAsync(gameName, blobService);
        //  VGame vGame = await VGame.Load(gameName, blobService);
        gameSetup.ConfirmGamemasterAuthority(requestor);
          WorldSetup worldSetup = await WorldSetup.RetrieveAsync(gameName, blobService);


        InitParms initParms = new InitParms((gameSetup.InitFileInfo is null) ? null :
        await InitParmsFile.RetrieveAsync(gameSetup.InitFileInfo, blobService));       //   GameStatus gameStatus = await GameStatus.RetrieveAsync(gameName, blobService);
        GameStatus gameStatus = GameStatus.Create(gameSetup, worldSetup, initParms);
        List<Nation> nations = await gameSetup.GatherNationsAsync(blobService);
  
        AssignNations();
        await FinalizeNationNamingAsync();
        ActivateNations();

        WorldStatus worldStatus = new WorldStatus(worldSetup, nations, initParms);
        ServerEconomicMgr econUpdater = new(gameSetup, gameStatus, worldSetup, worldStatus, nations);
        //  econUpdater.DetermineResults();
        econUpdater.UpdateForNextSeason();  // NEEDED ONLY FOR REPORTS ???

        gameSetup.GameState = GameState.Activated;

        List<BlobDescriptor> descriptors = [];
        descriptors.Add(gameSetup.BlobDescriptor());
        descriptors.Add(gameStatus.BlobDescriptor());
        descriptors.Add(worldStatus.BlobDescriptor());
        foreach (Nation nation in nations) descriptors.Add(nation.BlobDescriptor());
        await blobService.SaveGroupAsync(descriptors);

        void AssignNations()
        {
            List<string> availableHomes = worldSetup.GatherAvailableHomes(nations);

            if (prescribedNations is false) Util.Shuffle(availableHomes);
            foreach (Nation nation in nations)
            {
                if (nation.HomeDistrict is null)
                {
                    nation.HomeDistrict = availableHomes[0];
                    availableHomes.RemoveAt(0);
                }
            }
        }
        async Task FinalizeNationNamingAsync()
        {
            NamingFile namingFile = await NamingFile.RetrieveAsync("test", blobService);
            List<NationNameOption> names = namingFile.NationNames;
            RemoveUsedNames();
            if (prescribedNations is false) Util.Shuffle(names);
            foreach (Nation nation in nations)
            {
                if (nation.Naming.IsNamed()) continue;
                if (names.Count <= 0) throw new AppException(ExceptionType.Endpoint, EndpointFailType.Invalid, "Too Many Nations");
                nation.Naming = new NationNaming()
                {
                    NationCode = nation.Identity.NationCode,
                    Name = names[0].Name,
                    Possessive = names[0].Possessive,
                    LeaderTitle = Util.PickRandomFromList(namingFile.LeaderTitles),
                    Government = Util.PickRandomFromList(namingFile.Governments)
                };
                names.RemoveAt(0);
            }
            void RemoveUsedNames()
            {
                foreach (Nation nation in nations)
                {
                    if (nation.Naming.Name is null) continue;
                    names.RemoveAll(n => n.Name == nation.Naming.Name);
                    names.RemoveAll(n => n.Possessive == nation.Naming.Possessive);
                }

            }
        }
        // void CreateDistrictStatuses()
        // {
        //     foreach (DistrictStatus district in game.Districts)
        //     {
        //         Nation? owningNation = nations.Find(n => n.HomeDistrict == district.Name);
        //         district.Activate(owningNation?.Identity.NationCode ?? 0);
        //     }
        // }
        // void CreateDistrictReports()
        // {
        //     List<NationNaming> namings = NationNaming.GatherNationNamings(nations);
        //     foreach (District district in game.Districts)
        //     {
        //         DistrictMgr mgr = new DistrictMgr(district, null);
        //         district.CreateReport(mgr, game.GameDate, null, namings, game.EconParms.VillageParms);
        //     }
        // }


        void ActivateNations()
        {
            foreach (Nation nation in nations)
            {
                nation.Activate(initParms);
            }
        }
        // void CreateNationReports()
        // {
        //     foreach (Nation nation in nations)
        //     {
        //         NationMgr mgr = new NationMgr(nation, game.Districts);
        //         nation.CreateReport(mgr, game.GameDate);
        //     }
        // }

    }
}