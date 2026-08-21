

using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Parameters;

public partial class NamingFile : IBlobObject
{
    // public const string ParmTypeIndicator = ".naming";
    // private static string FullFileName(string rootName) { return rootName + ParmTypeIndicator + BlobNaming.Json; }
    private string BlobPath() { return BlobService.CreateBlobPath(Folders.Parms, null, ParmFileInfo.FileName()); }
    private static string BlobPath(string rootName) { return BlobService.CreateBlobPath(Folders.Parms, null, ParmFileInfo.FileName(rootName, ParmFileType.Naming)); }
    public BlobDescriptor BlobDescriptor() { return new BlobDescriptor(BlobPath(), this); }
    public void Validate()
    {

    }
    public Task SaveAsync(BlobService blobService)
    {
        BlobDescriptor descriptor = new(BlobPath(), this);
        return blobService.SaveAsync(descriptor);
    }
    public static async Task<NamingFile> RetrieveAsync(string fileName, BlobService blobService)
    {
        try
        {
            return await blobService.RetrieveAsync<NamingFile>(BlobPath(fileName));
        }
        catch { }
        return defaultNamingFile;
    }
    private static NamingFile defaultNamingFile = new NamingFile()
    {
        ParmFileInfo = new ParmFileInfo()
        {
            RootName = "Default",
            Version = "0",
            Type = ParmFileType.Naming
        },
        LeaderTitles = new List<string>()
            {
                "Regent",
                "Monarch",
                "Chancellor"
            },
        NationNames = new List<NationNameOption>()
            {
                new NationNameOption("Xray", "Xrayin"),
                new NationNameOption("Upsilon", "Upsilonian"),
                new NationNameOption("Targaryen", "Targaryen's")
            }
    };
}