
using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class NationEndpoints
{
    public static async Task ProcessGet(Nation nation, VGame vGame, BlobService blobService, NationResponse response)
    {
        response.Identity = new NationIdentity(nation.Identity.GameName, nation.Identity.NationCode);
        response.Naming = nation.Naming;
        response.HomeDistrict = nation.HomeDistrict;
        response.IsGameActivated = vGame.GameState == GameState.Activated;
        response.AvailableHomes = await GatherHomeDistrictOptions();

        async Task<List<string>> GatherHomeDistrictOptions()
        {
            List<string> availHomes = [];
            if (nation.HomeDistrict is not null) availHomes.Add(nation.HomeDistrict);
            List<Nation> allNations = await vGame.GatherNationsConfirmDatesAsync(blobService);
            availHomes.AddRange(vGame.GatherAvailableHomes(allNations));
            return availHomes;
        }

    }
}
// public partial class NationNaming
// {
//     // public NationNaming(Nation nation)
//     // {
//     //     NationCode = nation.Identity.NationCode;
//     //     Name = nation.Naming.Name;
//     //     Possessive = nation.Naming.Possessive;
//     //     Government = nation.Naming.Government;
//     //     LeaderTitle = nation.Naming.LeaderTitle;
//     // }

//     // public bool NamingUpdate(NationName suggestion, NamingFile namingFile)
//     // {
//     //     bool suggestionUsed = (Name is null || Possessive is null) ? true : false;
//     //     if (Name is null) Name = suggestion.Name;
//     //     if (Possessive is null) Possessive = suggestion.Possessive;
//     //     if (LeaderTitle is null) LeaderTitle = Util.PickRandomFromList(namingFile.LeaderTitles);
//     //     if (Government is null) Government = Util.PickRandomFromList(namingFile.Governments);
//     //     return suggestionUsed;
//     // }
//     // public NationNaming(int nationCode)
//     // {
//     //     NationCode = nationCode;
//     // }
// }
