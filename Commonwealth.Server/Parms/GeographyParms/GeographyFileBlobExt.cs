
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Parameters;

public partial class GeographyFile : IBlobObject
{
    // public const string ParmTypeIndicator = ".geog";
    // private static string FullFileName(string rootName) { return rootName + ParmTypeIndicator + BlobNaming.Json; }
    private string BlobPath() { return BlobService.CreateBlobPath(Folders.Parms, null, ParmFileInfo.FileName()); }
    private static string BlobPath(ParmFileInfo info) { return BlobService.CreateBlobPath(Folders.Parms, null, info.FileName()); }
    public BlobDescriptor BlobDescriptor() { return new BlobDescriptor(BlobPath(), this); }
    public void Validate()
    {

    }
    public Task SaveAsync(BlobService blobService)
    {
        BlobDescriptor descriptor = new(BlobPath(), this);
        return blobService.SaveAsync(descriptor);
    }
    public static async Task<GeographyFile> RetrieveAsync(ParmFileInfo? info, BlobService blobService)
    {
        if (info is null) info = GeographyFile.DefaultFileInfo;
        GeographyFile file = await blobService.RetrieveAsync<GeographyFile>(BlobPath(info));
        return file;
    }
}