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
        BlobService blobService,
        ResponseBase response
    )
    {
        Game game = await Game.RetrieveAsync(gameName, blobService);
        game.ConfirmGamemasterAuthority(requestor);

        List<Nation> nations = await game.GatherNationsAsync(blobService);
        NamingFile namingFile = await NamingFile.RetrieveAsync("test", blobService);
        InitParms initParms = new InitParms((game.InitFileInfo is null) ? null :
            await InitParmsFile.RetrieveAsync(game.InitFileInfo, blobService));

        AssignNations();
        FinalizeNationNaming();
        game.Activate();
        ActivateDistricts();  // Must Follow AssignNations
        ActivateNations();
        ServerEconomicMgr econUpdater = new(game, nations);
        econUpdater.DetermineResults();
        econUpdater.UpdateForNextSeason();

        List<BlobDescriptor> descriptors = [];
        descriptors.Add(game.BlobDescriptor());
        foreach (Nation nation in nations) descriptors.Add(nation.BlobDescriptor());
        await blobService.SaveGroupAsync(descriptors);

        void AssignNations()
        {
            List<string> availableHomes = game.GatherAvailableHomes(nations);

            if (game.HasPrescribedNationAssignments is false) Util.Shuffle(availableHomes);
            foreach (Nation nation in nations)
            {
                if (nation.HomeDistrict is null)
                {
                    nation.HomeDistrict = availableHomes[0];
                    availableHomes.RemoveAt(0);
                }
            }
        }
        void FinalizeNationNaming()
        {
            List<NationNameOption> names = namingFile.NationNames;
            RemoveUsedNames();
            if (game.HasPrescribedNationAssignments is false) Util.Shuffle(names);
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
        void ActivateDistricts()
        {
            foreach (District district in game.Districts)
            {
                Nation? owningNation = nations.Find(n => n.HomeDistrict == district.Name);
                district.Activate(owningNation?.Identity.NationCode ?? 0);
            }
        }
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
                nation.Activate(game, initParms);
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