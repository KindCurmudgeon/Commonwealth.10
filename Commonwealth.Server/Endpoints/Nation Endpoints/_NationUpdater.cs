using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;
public static partial class NationEndpoints
{
    public static async Task ProcessUpdate(Nation nation, NationRequest editRequest, BlobService blobService, NationResponse response)
    {
        bool isUpdated = false;
        VGame game = await VGame.Load(nation.Identity!.GameName, blobService);
        if (game.GameState == GameState.Created)
        {
            await UpdateHomeDistrict();
        }
        UpdateNaming();
        UpdateAcceptance();
        if (isUpdated)
        {
            await nation.SaveAsync(blobService);
            if (editRequest.RequestType != NationRequestType.AcceptReject)
            {
                response.AddMessage($"Nation '{nation.Naming.Name} has been udpated!");
            }
        }
        else response.AddMessage("No Updates");

        async Task UpdateHomeDistrict()
        {
            if (editRequest.HomeDistrict is null) return;
            nation.HomeDistrict = editRequest.HomeDistrict;
            isUpdated = true;
        }
        void UpdateNaming()
        {
            if (editRequest.Naming is null) return;
            nation.Naming.Name = editRequest.Naming.Name;
            nation.Naming.Possessive = editRequest.Naming.Possessive;
            nation.Naming.LeaderTitle = editRequest.Naming.LeaderTitle;
            nation.Naming.Government = editRequest.Naming.Government;
            isUpdated = true;
        }
        void UpdateAcceptance()
        {
            if (editRequest.IsAccepted is null) return;
            LineupState original = nation.LineupState;
            nation.LineupState = editRequest.IsAccepted == true ? LineupState.Accepted : LineupState.Regrets;
            if (original != nation.LineupState) isUpdated = true;
        }
    }

}

