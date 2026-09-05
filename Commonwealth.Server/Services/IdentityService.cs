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

    public async Task<ProfileResponse?> GetProfileDTOAsync(ProfileRequest request)
    {
        try
        {
            HttpResponseMessage message = await Client.PostAsJsonAsync(IdentityEndpoint.Profile, request, options);
            ProfileResponse? response = await message.Content.ReadFromJsonAsync<ProfileResponse>();
            return response;
        }
        catch
        {
            return null;
        }
    }
}
