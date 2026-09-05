using Utilities;

namespace Data;

public partial class UserProfile
{

     public static string FullFileName(string id) { return id + BlobNaming.Json; }
     //   public string BlobPath() { return BlobService.CreateBlobPath(Folders.IDP, null, FullFileName()); }
     public static string BlobPath(Guid id) { return BlobService.CreateBlobPath(Folders.IDP, null, FullFileName(id.ToString())); }
     public BlobDescriptor BlobDescriptor() { return new BlobDescriptor(BlobPath(Id), this); }

     public Task SaveAsync(BlobService blobService)
     {
          BlobDescriptor descriptor = new(BlobPath(Id), this);
          return blobService.SaveAsync(descriptor);
     }
     public static async Task<UserProfile> RetrieveAsync(Guid id, BlobService blobService)
     {
          return await blobService.RetrieveAsync<UserProfile>(BlobPath(id));
     }
     public static async Task RemoveAsync(Guid id, BlobService blobService)
     {
          await blobService.RemoveJsonAsync(BlobPath(id));
     }
     public void Validate() { }
}