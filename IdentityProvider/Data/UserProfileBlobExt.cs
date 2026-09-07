using Utilities;

namespace Data;

public partial class UserProfile
{

     public static string FullFileName(string userName) { return userName + BlobNaming.Json; }
     //   public string BlobPath() { return BlobService.CreateBlobPath(Folders.IDP, null, FullFileName()); }
     public static string BlobPath(string userName) { return BlobService.CreateBlobPath(Folders.Users, null, FullFileName(userName)); }
     public BlobDescriptor BlobDescriptor() { return new BlobDescriptor(BlobPath(UserName), this); }

     public async Task<bool> UserExists(string userName, BlobService blobService)
     {
          return await blobService.IsExisting(BlobPath(userName));
     }
     public Task SaveAsync(BlobService blobService)
     {
          BlobDescriptor descriptor = new(BlobPath(UserName), this);
          return blobService.SaveAsync(descriptor);
     }
     public static async Task<UserProfile> RetrieveAsync(string userName, BlobService blobService)
     {
          return await blobService.RetrieveAsync<UserProfile>(BlobPath(userName));
     }
     public static async Task RemoveAsync(string userName, BlobService blobService)
     {
          await blobService.RemoveJsonAsync(BlobPath(userName));
     }
     public void Validate() { }
}