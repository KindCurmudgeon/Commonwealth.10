using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Commonwealth.Server.Parameters;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;

namespace Commonwealth.Server.Data;

public partial class DistrictSetup
{
    public required string Name { get; set; }
    public required string Region { get; set; }
    public required List<string> Connections { get; set; }
    public List<Feature>? Features { get; set; }
    public List<string>? Resources { get; set; } // Setup
    public List<string>? AllowedVillages { get; set; } //Setup
    [JsonConstructor] public DistrictSetup() { }
    [SetsRequiredMembers]
    public DistrictSetup(DistrictParm geog, List<DistrictParm>? seas)
    {
        Name = geog.Name;
        Region = geog.Region ?? "Sea";
        Connections = AssembleConnections(geog, seas);
        Features = Util.StringToEnumList<Feature>(geog.Features);

        // district.UpdateFoodMetrics(econParms);
        // switch (district.FoodMetrics?.FoodStatus)
        // {
        //     case FoodStatus.RATIONING: district.StaffAvailable = (int)(district.Population * 0.5); break;
        //     case FoodStatus.FAMINE: district.StaffAvailable = 0; break;
        //     default: district.StaffAvailable = district.Population; break;
        // }

        List<string> AssembleConnections(DistrictParm geog, List<DistrictParm>? seas)
        {
            List<string> connections = geog.LandConnections ?? [];
            AddSeaConnections(geog, seas, 2);
            return connections;

            void AddSeaConnections(DistrictParm sourceParm, List<DistrictParm>? seas, int depthLevel)
            {
                List<string> seasChecked = [];

                foreach (string first in sourceParm?.SeaConnections ?? [])
                {
                    ProcessSeaLayer(first, depthLevel);
                }
                void ProcessSeaLayer(string seaName, int depth)
                {
                    if (depth <= 0) return;
                    seasChecked.Add(seaName);
                    DistrictParm? seaParm = seas?.Find(s => s.Name == seaName);
                    foreach (string connection in seaParm?.LandConnections ?? [])
                    {
                        connections!.AddIfNotDuplicate(connection);
                    }
                    foreach (string connection in seaParm?.SeaConnections ?? [])
                    {
                        if (connection == sourceParm?.Name) continue;
                        if (seasChecked.Contains(connection)) continue;
                        ProcessSeaLayer(connection, depth - 1);
                    }
                }
            }
        }
    }
 

}


