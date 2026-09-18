using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Parameters;
using Commonwealth.Server.Utilities;

namespace Commonwealth.Server.Data;

public partial class WorldStatus : IBlobObject
{
     public required string GameName { get; set; }
     public required List<DistrictStatus> Districts { get; set; }
     [JsonConstructor] public WorldStatus() { }
     
     [SetsRequiredMembers] public WorldStatus(WorldSetup worldSetup, List<Nation> nations, InitParms initParms)
     {
          GameName = worldSetup.GameName;
          Districts = [];
          foreach (DistrictSetup setup in worldSetup.Districts)
          {
               Districts.Add(new DistrictStatus(setup, initParms));
          }
          foreach (Nation nation in nations)
          {
               DistrictStatus status = GetDistrict(nation.HomeDistrict);
               status.Owner = nation.Identity.NationCode;
          }
     }
}




