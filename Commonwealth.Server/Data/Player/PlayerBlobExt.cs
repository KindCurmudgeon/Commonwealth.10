using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Data;

public partial class Player : IBlobObject
{
    private static string FullFileName(string userName) { return userName + BlobNaming.Json; }
    private string BlobPath() { return BlobService.CreateBlobPath(Folders.Players, null, FullFileName(UserName)); }
    public static string BlobPath(string userName) { return BlobService.CreateBlobPath(Folders.Players, null, FullFileName(userName)); }
    public BlobDescriptor BlobDescriptor() { return new BlobDescriptor(BlobPath(), this); }
    public void Validate()
    {
    }

    public static string AllUsersPrefix() { return BlobService.CreateBlobPrefix(Folders.Players, null, null); }

    public Task SaveAsync(BlobService blobService)
    {
        BlobDescriptor descriptor = new(BlobPath(), this);
        return blobService.SaveAsync(descriptor);
    }
    public static async Task<Player> RetrieveAsync(string userName, BlobService blobService)
    {
        try
        {
            return await blobService.RetrieveAsync<Player>(BlobPath(userName));
        }
        catch (Exception ex)
        {
            // Handle the exception, e.g., log it or throw a custom exception
            ex.AddFailedItem($"Player '{userName}'");
            throw;
        }
    }
}