using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;


namespace Commonwealth.Server.Data;
public partial class Nation : IBlobObject
{
    private static string FullFileName(NationIdentity identity)
    {
        return identity.GameName + BlobNaming.NationSuffix + identity.NationCode.ToString() + BlobNaming.Json;
    }
    public string BlobPath() { return BlobService.CreateBlobPath(Folders.Games, null, FullFileName(Identity)); }
    public static string BlobPath(NationIdentity identity) { return BlobService.CreateBlobPath(Folders.Games, null, FullFileName(identity)); }
    public BlobDescriptor BlobDescriptor() { return new BlobDescriptor(BlobPath(), this); }
    public static BlobDescriptor BlobDescriptorRemove(NationIdentity identity)
    {
        return new(BlobPath(identity), true);
    }

    public static string BlobPrefix(string gameName) { return BlobService.CreateBlobPath(Folders.Games, null, gameName + BlobNaming.NationSuffix); }

    public void Validate()
    {
    }

    public Task SaveAsync(BlobService blobService)
    {
        BlobDescriptor descriptor = new(BlobPath(), this);
        return blobService.SaveAsync(descriptor);
    }
    public static async Task<Nation> RetrieveAsync(NationIdentity identity, BlobService blobService)
    {
        try
        {
            return await blobService.RetrieveAsync<Nation>(BlobPath(identity));
        }
        catch (Exception ex)
        {
            ex.AddFailedItem("Nation");
            throw;
        }
    }
    public static async Task RemoveAsync(NationIdentity identity, BlobService blobService)
    {
        await blobService.RemoveJsonAsync(BlobPath(identity));
    }
}