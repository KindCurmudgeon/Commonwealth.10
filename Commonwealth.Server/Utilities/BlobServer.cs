using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure;
using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Commonwealth.Server.Endpoints.ExceptionHandling;


namespace Commonwealth.Server.Utilities;

public class BlobService
{
    private static string BlobContainerName = "goc-files";
    public required BlobContainerClient ContainerClient { get; set; }
    [SetsRequiredMembers]
    public BlobService(BlobServiceClient blobServiceClient)
    {
        ContainerClient = blobServiceClient.GetBlobContainerClient(BlobContainerName);
        ContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
    }
    public async Task SaveAsync(BlobDescriptor descriptor)
    {
        if (descriptor.Obj is null) throw new AppException(ExceptionType.Blob, BlobFailType.Unexpected, descriptor.PathName);
        string path = descriptor.PathName;
        Object blobObject = descriptor.Obj;
        try
        {
            BlobClient blobClient = ContainerClient.GetBlobClient(path);
            string jsonData = JsonSerializer.Serialize(blobObject);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData));
            await blobClient.UploadAsync(stream, overwrite: true);
        }
        catch (Exception ex)
        {
            throw new AppException(ExceptionType.Blob, BlobFailType.TroubleWrite, ex);
        }
    }
    public async Task<T?> RetrieveIfExistAsync<T>(string pathName) where T : IBlobObject
    {
        try
        {
            T file = await RetrieveAsync<T>(pathName);
            return file;
        }
        catch { return default; }
    }
    public async Task<T> RetrieveAsync<T>(string pathName) where T : IBlobObject
    {
        try
        {
            BlobClient blobClient = ContainerClient.GetBlobClient(pathName);

            string jsonContent;
            using (MemoryStream stream = new())
            {
                await blobClient.DownloadToAsync(stream);
                stream.Position = 0; // Reset stream position to the beginning
                using (StreamReader reader = new(stream))
                {
                    jsonContent = await reader.ReadToEndAsync();
                }
            }
            T? result = JsonSerializer.Deserialize<T>(jsonContent);
            ValidateFileContents(result);
            return result!;
        }
        catch (RequestFailedException ex)
        {
            Console.WriteLine(ex.Message);
            throw new AppException(ExceptionType.Blob, BlobFailType.NotFound, pathName);
        }
        catch (JsonException ex)
        {
            throw new AppException(ExceptionType.Blob, BlobFailType.JSON, ex);
        }
        catch (Exception ex)
        {
            throw new AppException(ExceptionType.Blob, BlobFailType.Unexpected, ex);
        }
        void ValidateFileContents(IBlobObject? result)
        {
            if (result is null) throw new AppException(ExceptionType.Blob, BlobFailType.JSON, (string?)null);
            result.Validate();
        }
    }
    public async Task<bool> SaveGroupAsync(List<BlobDescriptor> descriptors)
    {
        try
        {
            foreach (BlobDescriptor descriptor in descriptors)
            {
                BlobClient blobClient = ContainerClient.GetBlobClient(descriptor.PathName);
                await blobClient.DeleteIfExistsAsync();
                if (descriptor.ToBeDeleted is true) continue;

                using (MemoryStream stream = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(stream, descriptor.Obj);
                    stream.Position = 0;

                    await blobClient.UploadAsync(
                        stream,
                        new BlobHttpHeaders { ContentType = "application/json" }
                    );
                }
            }
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> IsExisting(string blobPath)
    {
        try
        {
            BlobClient blobClient = ContainerClient.GetBlobClient(blobPath);
            bool result = await blobClient.ExistsAsync();
            return result;
        }
        catch (Exception ex)
        {
            string x = ex.Message;
            throw new AppException(ExceptionType.Blob, BlobFailType.BlobFailure, blobPath);
        }
    }
    public async Task RemoveJsonAsync(string blobPath)
    {
        try
        {
            BlobClient blobClient = ContainerClient.GetBlobClient(blobPath);
            await blobClient.DeleteAsync();
        }
        catch { }
    }
    public async Task RemoveWithPrefix(string parentFolder, string? childFolder, string? prefix)
    {
        string template = CreateBlobPrefix(parentFolder, childFolder, prefix);
        List<string> files = await GetFileNamesWithPrefix(template);
        try
        {
            foreach (string fileName in files)
            {
                BlobClient blobClient = ContainerClient.GetBlobClient(fileName);
                await blobClient.DeleteIfExistsAsync();
            }
        }
        catch (Exception ex)
        {
            throw new AppException(ExceptionType.Blob, BlobFailType.Remove, ex);
        }
    }
    public async Task<List<string>> GetFileNamesWithPrefix(string prefix)
    {
        List<string> fileNames = [];
        try
        {
            await foreach (BlobItem item in ContainerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, prefix, CancellationToken.None))
            {
                fileNames.Add(item.Name);
            }
            return fileNames;
        }
        catch (Exception ex) { throw new AppException(ExceptionType.Blob, BlobFailType.Unexpected, ex); }
    }
    public async Task<string?> GetJsonStringAsync(string pathName)
    {
        try
        {
            BlobClient blobClient = ContainerClient.GetBlobClient(pathName);

            string? jsonContent = null;
            try
            {
                using (MemoryStream stream = new())
                {
                    await blobClient.DownloadToAsync(stream);
                    stream.Position = 0; // Reset stream position to the beginning
                    using (StreamReader reader = new(stream))
                    {
                        jsonContent = await reader.ReadToEndAsync();
                        return jsonContent;
                    }
                }
            }
            catch (Exception ex) { jsonContent = ex.Message; }
            return jsonContent;
        }
        catch (RequestFailedException ex)
        {
            throw new AppException(ExceptionType.Blob, BlobFailType.NotFound, ex);
        }
        catch (JsonException ex)
        {
            throw new AppException(ExceptionType.Blob, BlobFailType.JSON, ex);
        }
        catch (Exception ex)
        {
            throw new AppException(ExceptionType.Blob, BlobFailType.Unexpected, ex);
        }
    }

    public static string CreateBlobPath(string parentFolder, string? childFolder, string fileName)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append($"{parentFolder.ToLower()}/");
        if (childFolder is not null) stringBuilder.Append($"{childFolder.ToLower()}/");
        stringBuilder.Append(fileName.ToLower());
        return stringBuilder.ToString();
    }
    public static string CreateBlobPrefix(string parentFolder, string? childFolder, string? prefix)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append($"{parentFolder.ToLower()}/");
        if (childFolder is not null) stringBuilder.Append($"{childFolder.ToLower()}/");
        if (prefix is not null) stringBuilder.Append(prefix.ToLower());
        return stringBuilder.ToString();
    }
    public static string ExtractFromFileName(string pathName)
    {
        string[] parts = pathName.Split('/');
        if (parts.Length == 0) return pathName;
        return $"{parts[1]}";
    }
}
public class BlobDescriptor
{
    public required string PathName { get; set; }
    public Object? Obj { get; set; }
    public bool? ToBeDeleted { get; set; }

    [SetsRequiredMembers] public BlobDescriptor(string filePath, object obj) { PathName = filePath; Obj = obj; }
    [SetsRequiredMembers] public BlobDescriptor(string filePath, bool? toBeDeleted) { PathName = filePath; Obj = null; ToBeDeleted = toBeDeleted; }
}
public interface IBlobObject
{
    //   public string BlobPath();
    public BlobDescriptor BlobDescriptor();
    public Task SaveAsync(BlobService blobService);
    public void Validate();
    //  public static abstract Task RetrieveAsync(string rootName, BlobService blobService);
}
public static class BlobServiceExtensions
{
    public static void AddBlobService(this WebApplicationBuilder builder, TokenCredential credential)
    {
        builder.Services.AddSingleton(serviceProvider =>
        {
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
            var connectionString = config.GetConnectionString("Blob")
                ?? throw new InvalidOperationException("Storage url is missing");
            return environment.IsDevelopment() ?
                new BlobServiceClient(connectionString) :
                new BlobServiceClient(new Uri(connectionString), credential);
        })
        .AddSingleton<BlobService>();
    }
    public static async Task<T> AddIfNotDuplicateAsync<T>(this List<BlobDescriptor> descriptors, string pathName, BlobService blobService) where T : IBlobObject
    {
        BlobDescriptor? descriptor = descriptors.Find(d => d.PathName == pathName);
        if (descriptor?.Obj is not null) return (T)descriptor.Obj!;
        T Obj = await blobService.RetrieveAsync<T>(pathName);
        descriptors.Add(new BlobDescriptor(pathName, Obj));
        return Obj;
    }
    public static async Task<T> RetrieveIfNotFoundAsync<T>(this List<BlobDescriptor> descriptors, string pathName, BlobService blobService) where T : IBlobObject
    {
        BlobDescriptor? found = descriptors.Find(d => d.PathName == pathName);
        if (found is not null) return (T)found.Obj!;
        T obj = await blobService.RetrieveAsync<T>(pathName);
        descriptors.Add(new BlobDescriptor(pathName, obj));
        return obj;
    }
}
