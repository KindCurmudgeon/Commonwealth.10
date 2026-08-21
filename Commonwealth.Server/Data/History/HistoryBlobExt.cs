using Commonwealth.Server.Utilities;

namespace Commonwealth.Server.Data;


public partial class History : IBlobObject
{
    private static string FullFileName(string gameName, int seasonCount)
    {
        return gameName + BlobNaming.HistorySuffix + seasonCount.ToString("D3") + BlobNaming.Json;
    }
    private string BlobPath() { return BlobService.CreateBlobPath(Folders.Games, null, FullFileName(Game.Name, Game.GameDate.Year)); }
    private static string BlobPath(string gameName, int seasonCount) { return BlobService.CreateBlobPath(Folders.Games, null, FullFileName(gameName, seasonCount)); }
    public BlobDescriptor BlobDescriptor() { return new BlobDescriptor(BlobPath(), this); }
    public void Validate()
    {
    }

    public Task SaveAsync(BlobService blobService)
    {
        BlobDescriptor descriptor = new (BlobPath(), this);
        return blobService.SaveAsync(descriptor);
    }
 
    public static async Task<History> RetrieveAsync(string gameName, int seasonCount, BlobService blobService)
    {
        string fileName = FileName(gameName, seasonCount);
        return await blobService.RetrieveAsync<History>(History.BlobPath(gameName, seasonCount));
        //    return await FileIO.ReadJsonDataAsync<History>(Folders.Games, fileName);

    }    
}