using System.Text.Json.Serialization;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;

namespace Commonwealth.Server.Data;

public partial class WorldSetup : IBlobObject
{
     public required string GameName { get; set; }
     public required List<DistrictSetup> Districts { get; set; }
     [JsonConstructor] public WorldSetup() { }
}


public partial class WorldSetup
{
     private static string FullFileName(string gameName) { return gameName + BlobNaming.WorldSetupSuffix + BlobNaming.Json; }
     private string BlobPath() { return BlobService.CreateBlobPath(Folders.Games, GameName, FullFileName(GameName)); }
     public static string BlobPath(string gameName)
     {
          return BlobService.CreateBlobPath(Folders.Games, gameName, FullFileName(gameName));
     }
     public BlobDescriptor BlobDescriptor() { return new BlobDescriptor(BlobPath(), this); }
     public void Validate() { }

     public Task SaveAsync(BlobService blobService)
     {
          BlobDescriptor descriptor = new(BlobPath(), this);
          return blobService.SaveAsync(descriptor);
     }
     public static async Task<WorldSetup> RetrieveAsync(string gameName, BlobService blobService)
     {
          try
          {
               return await blobService.RetrieveAsync<WorldSetup>(BlobPath(gameName));
          }
          catch (Exception ex)
          {
               // Handle the exception, e.g., log it or throw a custom exception
               ex.AddFailedItem($"World Setup: '{gameName}'");
               throw;
          }
     }
}
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
}
