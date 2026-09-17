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


public partial class WorldStatus
{
     private static string FullFileName(string gameName) { return gameName + BlobNaming.WorldStatusSuffix + BlobNaming.Json; }
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
     public static async Task<WorldStatus> RetrieveAsync(string gameName, BlobService blobService)
     {
          try
          {
               return await blobService.RetrieveAsync<WorldStatus>(BlobPath(gameName));
          }
          catch (Exception ex)
          {
               // Handle the exception, e.g., log it or throw a custom exception
               ex.AddFailedItem($"World Status: '{gameName}'");
               throw;
          }
     }
}

public partial class WorldStatus
{
     public List<DistrictStatus> GetOwnedDistricts(int nationCode)
     {
          return Districts.Where(d => d.Owner == nationCode).ToList();
     }
     public DistrictStatus GetDistrict(string? districtName)
     {
          DistrictStatus? district =  Districts.Find(d => d.Name == districtName);
          if (district == null) throw new AppException(ExceptionType.Blob, BlobFailType.NotFound, $"District: {districtName ?? "null"}");
          return district;
     }
}