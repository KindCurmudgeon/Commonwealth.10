using Commonwealth.Server.Utilities;

namespace Commonwealth.Server.Data;

public partial class GameSetup : GameAuthority, IBlobObject
{

    private static string FullFileName(string gameName) { return gameName.ToLower() + BlobNaming.GameSetupSuffix + BlobNaming.Json; }
    private string BlobPath() { return BlobService.CreateBlobPath(Folders.Games, GameName, FullFileName(GameName)); }
    public BlobDescriptor BlobDescriptor() { return new BlobDescriptor(BlobPath(), this); }
    public static string BlobPath(string gameName) { return BlobService.CreateBlobPath(Folders.Games, gameName, FullFileName(gameName)); }
    public static string AllGameFilesBlobPrefix(string gameName) { return BlobService.CreateBlobPrefix(Folders.Games, null, gameName); }
    public void Validate() { }

    public Task SaveAsync(BlobService blobService)
    {
        BlobDescriptor descriptor = new(BlobPath(), this);
        return blobService.SaveAsync(descriptor);
    }
    public static async Task<GameSetup> RetrieveAsync(string gameName, BlobService blobService)
    {
        GameSetup setup = await blobService.RetrieveAsync<GameSetup>(BlobPath(gameName));
        return setup;
    }

}
