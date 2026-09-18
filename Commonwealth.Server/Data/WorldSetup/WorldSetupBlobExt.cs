using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;

namespace Commonwealth.Server.Data;
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