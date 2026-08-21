
using Commonwealth.Shared.EndpointDTOs;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;


namespace Commonwealth.Client.Services;

public class ApiService
{
    private readonly HttpClient Client;
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
            var X = await Client.PostAsJsonAsync(endpoint, request);
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


