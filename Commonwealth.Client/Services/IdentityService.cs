using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    public async Task<string?> GetToken(string userName, string password)
    {
        AuthenticateRequest request = new AuthenticateRequest()
        {
            RequestType = AuthenticateRequestType.AUTHENTICATE,
            AuthProfileDTO = new AuthProfileDTO()
            {
                UserName = userName,
                Password = password
            }
        };
        try
        {
            HttpResponseMessage message = await Client.PostAsJsonAsync(IdentityEndpoint.Authenticate, request, options);
            AuthenticateResponse? response = await message.Content.ReadFromJsonAsync<AuthenticateResponse>();
            return response?.Token;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while fetching token: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> RegisterNewUser(AuthProfileDTO authProfileDTO)
    {
        AuthenticateRequest request = new()
        {
            RequestType = AuthenticateRequestType.REGISTER,
            AuthProfileDTO = authProfileDTO
        };
        try
        {
            HttpResponseMessage message = await Client.PostAsJsonAsync(IdentityEndpoint.Authenticate, request, options);
            AuthenticateResponse? response = await message.Content.ReadFromJsonAsync<AuthenticateResponse>();
            return response?.Token;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while Registering: {ex.Message}");
            return null;
        }
    }
}
