
using Commonwealth.Shared.EndpointDTOs;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Commonwealth.Client.Services;

public class ApiService
{
    private readonly HttpClient Client;
    JsonSerializerOptions options = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Matching your API standard
    };
    public SharedDataService? DataService { get; set; }
    //   public MessagesModal? MessageAlert;
    public ApiService(HttpClient client)
    {
        Client = client;
    }

    public void SetDataService(SharedDataService dataService)
    {
        DataService = dataService;
    }

    public async Task<Res?> SendRequest<Req, Res>(string endpoint, Req request)
    {
        try
        {
            endpoint = endpoint.Replace("/", "");
            var X = await Client.PostAsJsonAsync(endpoint, request, options);
            Res? response = await X.Content.ReadFromJsonAsync<Res>();

            ResponseBase? parent = response as ResponseBase;
            DataService?.CheckAlerts(parent);

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API Service Error: {ex.Message}");
            return default;
        }

        //       return await X.Content.ReadFromJsonAsync<Res>();
    }
}


