
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Parameters;

public partial class EconParmsFile : IBlobObject
{
    public const string ParmTypeIndicator = ".econ";
    private static string FullFileName(string rootName) { return rootName + ParmTypeIndicator + BlobNaming.Json; }
    public string BlobPath() { return BlobService.CreateBlobPath(Folders.Parms, null, ParmFileInfo.FileName()); }
    public static string BlobPath(ParmFileInfo info)
    {
        return BlobService.CreateBlobPath(Folders.Parms, null, info.FileName());
    }

    // private string BlobPath() { return BlobService.CreateBlobPath(Folders.Parms, null, ParmFileInfo.FileName()); }
    // private static string BlobPath(string rootName) { return BlobService.CreateBlobPath(Folders.Parms, null, ParmFileInfo.FileName(rootName, ParmFileType.Econ)); }
    public BlobDescriptor BlobDescriptor() { return new BlobDescriptor(BlobPath(), this); }
    public void Validate()
    {

    }
    public Task SaveAsync(BlobService blobService)
    {
        BlobDescriptor descriptor = new(BlobPath(), this);
        return blobService.SaveAsync(descriptor);
    }
    public static async Task<EconParmsFile?> RetrieveAsync(ParmFileInfo? info, BlobService blobService)
    {
        if (info is null) return null;
        return await blobService.RetrieveIfExistAsync<EconParmsFile>(BlobPath(info));
    }
}