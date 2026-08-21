using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;

namespace Commonwealth.Server.Data;

public partial class User : IBlobObject
{
   private static string FullFileName(string userName) { return userName.ToLower() + BlobNaming.Json; }
    private string BlobPath() { return BlobService.CreateBlobPath(Folders.Users, null, FullFileName(UserName)); }
    public static string BlobPath(string userName) { return BlobService.CreateBlobPath(Folders.Users, null, FullFileName(userName)); }
    public BlobDescriptor BlobDescriptor() { return new BlobDescriptor(BlobPath(), this); } 
        public void Validate()
    {
    }

    public static string AllUsersPrefix() {return BlobService.CreateBlobPrefix(Folders.Users, null,null);}
        public Task SaveAsync(BlobService blobService)
    {
        BlobDescriptor descriptor = new (BlobPath(), this);
        return blobService.SaveAsync(descriptor);
    }
    public static async Task<User> RetrieveAsync(string userName, BlobService blobService)
    {
        try
        {
            return await blobService.RetrieveAsync<User>(BlobPath(userName));
        }
        catch (Exception ex)
        {
            // Handle the exception, e.g., log it or throw a custom exception
            ex.AddFailedItem($"User: '{userName}'");
            throw;
        }
    }
}