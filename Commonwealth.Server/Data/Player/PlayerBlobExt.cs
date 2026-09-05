using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Data;

public partial class Player : IBlobObject
{
    private static string FullFileName(Guid id) { return id.ToString() + BlobNaming.Json; }
    private string BlobPath() { return BlobService.CreateBlobPath(Folders.Players, null, FullFileName(Id)); }
    public static string BlobPath(Guid id) { return BlobService.CreateBlobPath(Folders.Players, null, FullFileName(id)); }
    public BlobDescriptor BlobDescriptor() { return new BlobDescriptor(BlobPath(), this); }
    public void Validate()
    {
    }

    public static string AllUsersPrefix() { return BlobService.CreateBlobPrefix(Folders.Players, null, null); }
    public static Task<bool> ExistsAsync(Guid guid, BlobService blobService)
    {
        return blobService.ExistsAsync(BlobPath(id));
    }
    public Task SaveAsync(BlobService blobService)
    {
        BlobDescriptor descriptor = new(BlobPath(), this);
        return blobService.SaveAsync(descriptor);
    }
    public static async Task<Player> RetrieveAsync(Guid userId, BlobService blobService)
    {
        try
        {
            return await blobService.RetrieveAsync<Player>(BlobPath(userId));
        }
        catch (Exception ex)
        {
            // Handle the exception, e.g., log it or throw a custom exception
            ex.AddFailedItem($"Player '{userId}'");
            throw;
        }
    }
}