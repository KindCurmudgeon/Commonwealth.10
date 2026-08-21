using System.Text.Json;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Parameters;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

// public class UploadEnvelope
// {
//     public string Type { get; set; } = "";
//     public JsonElement Payload { get; set; }
// }

// app.MapPost("/api/upload-json-raw", async (HttpRequest request) =>
// {
//     var envelope = await JsonSerializer.DeserializeAsync<UploadEnvelope>(request.Body);
//     if (envelope == null || string.IsNullOrWhiteSpace(envelope.Type))
//         return Results.BadRequest("Missing type.");

//     var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

//     return envelope.Type.ToLower() switch
//     {
//         "customer" => Results.Ok(new { type = envelope.Type, data = envelope.Payload.Deserialize<Customer>(options) }),
//         "order"    => Results.Ok(new { type = envelope.Type, data = envelope.Payload.Deserialize<Order>(options) }),
//         "product"  => Results.Ok(new { type = envelope.Type, data = envelope.Payload.Deserialize<Product>(options) }),
//         "invoice"  => Results.Ok(new { type = envelope.Type, data = envelope.Payload.Deserialize<Invoice>(options) }),
//         _          => Results.BadRequest("Unknown type")
//     };
// });
public static class AdminUploadParmsEnpoint
{
    public static void AdminUpdateParms(this IEndpointRouteBuilder app)
    {
        app.MapPost(Sub.UpdateParms, async (
            ParmUploadRequest request,
           BlobService blobService) =>
        {
            ParmUploadResponse response = new();
            // var envelope = await JsonSerializer.DeserializeAsync<ParmUploadRequest>(request.Payload);
            // if (envelope == null || string.IsNullOrWhiteSpace(envelope.Type)) return Results.Ok("Bad content");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            try
            {
                switch (request.Type.ToLower())
                {
                    case ParmFileType.Econ:
                        EconParmsFile econFile = JsonSerializer.Deserialize<EconParmsFile>(request.Payload, options) ??
                            throw new AppException(ExceptionType.Endpoint, EndpointFailType.FailedUpload, "Econ Parms");
                        await econFile.SaveAsync(blobService);
                        break;
                    case ParmFileType.Geog:
                        GeographyFile geogFile = JsonSerializer.Deserialize<GeographyFile>(request.Payload, options) ??
                            throw new AppException(ExceptionType.Endpoint, EndpointFailType.FailedUpload, "Geog Parms");
                        await geogFile.SaveAsync(blobService);
                        break;
                    case ParmFileType.Init:
                        InitParmsFile initParmsFile = JsonSerializer.Deserialize<InitParmsFile>(request.Payload, options) ??
                            throw new AppException(ExceptionType.Endpoint, EndpointFailType.FailedUpload, "Season Parms");
                        await initParmsFile.SaveAsync(blobService);
                        break;
                    case ParmFileType.Naming:
                        NamingFile namingFile = JsonSerializer.Deserialize<NamingFile>(request.Payload, options) ??
                            throw new AppException(ExceptionType.Endpoint, EndpointFailType.FailedUpload, "Naming");
                        await namingFile.SaveAsync(blobService);
                        break;
                    default: response.AddError("Unrecogized Parm File Type"); break;
                }
            }
            catch (Exception ex)
            {
                response.AddError(ex.Message);
            }
            return Results.Ok(response);
        });
    }
}