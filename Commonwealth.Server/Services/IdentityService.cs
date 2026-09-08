using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Commonwealth.Shared.EndpointDTOs;
using IdentityProvider.EndpointDTOs;


namespace Identity.Client.Service;

public class IdentityService
{
    private readonly HttpClient Client;
    JsonSerializerOptions options { get; set; }

    public IdentityService(HttpClient client)
    {
        Client = client;
        options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Matching your API standard
        };
    }

    public async Task<string?> GetUnverifiedTokenAsync(string userName)
    {
        AuthenticateRequest request = new ()
        {
            RequestType = (AuthenticateRequestType) 99,
            AuthProfileDTO = new() {UserName = userName, Password = string.Empty}
        };
        try
        {
            HttpResponseMessage message = await Client.PostAsJsonAsync(IdentityEndpoint.Authenticate, request, options);
            AuthenticateResponse? response = await message.Content.ReadFromJsonAsync<AuthenticateResponse>();
            return response?.Token;
        }
        catch
        {
            return null;
        }
    }
}
