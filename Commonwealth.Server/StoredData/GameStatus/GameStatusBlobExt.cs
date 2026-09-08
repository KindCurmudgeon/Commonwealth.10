using Commonwealth.Server.Utilities;

namespace Commonwealth.Server.Data;

public partial class GameStatus : IBlobObject
{
    private static string FullFileName(string gameName) { return gameName.ToLower() + BlobNaming.GameStatusSuffix + BlobNaming.Json; }
    private string BlobPath() { return BlobService.CreateBlobPath(Folders.Games, null, FullFileName(Name)); }
    public BlobDescriptor BlobDescriptor() { return new BlobDescriptor(BlobPath(), this); }
    private static string BlobPath(string gameName) { return BlobService.CreateBlobPath(Folders.Games, null, FullFileName(gameName)); }
    public static string AllGameFilesBlobPrefix(string gameName) { return BlobService.CreateBlobPrefix(Folders.Games, null,gameName); }
    public static string AllGamesBlobPrefix() { return BlobService.CreateBlobPrefix(Folders.Games, null, null); }
    public void Validate() {}

    public Task SaveAsync(BlobService blobService)
    {
        BlobDescriptor descriptor = new(BlobPath(), this);
        return blobService.SaveAsync(descriptor);
    }
    public static async Task<GameStatus> RetrieveAsync(string gameName, BlobService blobService)
    {
        GameStatus status = await blobService.RetrieveAsync<GameStatus>(BlobPath(gameName));
        return status;
    }

}
