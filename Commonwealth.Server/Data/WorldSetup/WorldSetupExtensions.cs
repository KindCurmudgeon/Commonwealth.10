using Commonwealth.Server.Data;
using Commonwealth.Server.Parameters;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.Common;

namespace Commonwealth.Server.Data;

public partial class WorldSetup : IBlobObject
{


     public DistrictSetup GetDistrict(string districtName)
     {
          return Districts.First(d => d.Name == districtName);
     }
     public List<string> GatherConnections(List<DistrictStatus> districts)
     {
          List<string> connections = [];
          foreach (DistrictStatus status in districts)
          {
               DistrictSetup setup = GetDistrict(status.Name);
               foreach (string connection in setup.Connections)
               {
                    string? match = setup.Connections.Find(c => c == connection);
                    if (match is not null) connections.AddIfNotDuplicate(connection);
               }
          }
          return connections;
     }
     public List<string> GatherAvailableHomes(List<Nation> nations)
     {
          List<string> available = Districts.Select(r => r.Name).ToList();
          List<string> taken = nations.Where(l => l.HomeDistrict is not null).Select(l => l.HomeDistrict!).ToList();
          foreach (string name in taken) available.Remove(name);
          return available;
     }

}