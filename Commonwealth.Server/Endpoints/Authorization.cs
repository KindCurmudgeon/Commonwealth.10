using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core.Cryptography;
using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
using Identity.Client.Service;
using IdentityProvider.EndpointDTOs;
using Microsoft.IdentityModel.Tokens;


namespace Commonwealth.Server.Endpoints;

public static class Authorization
{
    //  private static string secretKey = "32140Huber9845YorkWoods697WoodCreek";
    private static string secretKey = string.Empty;
    public static void AuthInitialize(this WebApplicationBuilder builder)
    {
        string? key = builder.Configuration["encryptionkey"];
        if (key is not null) secretKey = key;

        builder.Services.AddHttpClient<IdentityService>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:5029/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
    }

    // private static string ValidateUser(RequestBase request)
    // {
    //     return ExtractUserNameFromToken(request.Token);
    // }
    // public static async Task<bool> ValidateAsAdministrator(RequestBase request, BlobService blobService)
    // {
    //     string userName = ValidateUser(request);
    //                     User user = await User.RetrieveAsync(userName, blobService);
    //     return user.IsAdministrator;
    // }
    public static ProfileDTO ExtractProfileDTOfromToken(string? token)
    {
        List<Claim> claims = ExtractClaimsFromToken(token);
        string? id = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value.ToString();
        string? userName = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value.ToString();
        string? eMail = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value.ToString();
        string? familyName = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.FamilyName)?.Value.ToString();
        string? givenName = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.GivenName)?.Value.ToString();
        if (id is null || userName is null) throw new AppException(ExceptionType.Auth, AuthFailType.InvalidToken, "");
        return new ProfileDTO()
        {
            UserId = Guid.Parse(id),
            UserName = userName,
            Email = eMail,
            FamilyName = familyName,
            GivenName = givenName
        };
    }



    private static List<Claim> ExtractClaimsFromToken(string? token)
    {
        //      if (token is null) throw new NullReferenceException("Token");
        try
        {
            // Define the token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "CWGServer", // Replace with your issuer

                ValidateAudience = true,
                ValidAudience = "CWGplayer", // Replace with your audience

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

                ValidateLifetime = true, // Ensure the token hasn't expired
                ClockSkew = TimeSpan.Zero // Optional: Adjust for clock skew
            };
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            // Create the token handler
            var tokenHandler = new JwtSecurityTokenHandler();


            // Validate the token
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            return principal.Claims.ToList();

        }
        catch (SecurityTokenExpiredException ex)
        {
            throw new AppException(ExceptionType.Auth, AuthFailType.ExpiredToken, ex);
        }
        catch (Exception ex)
        {
            throw new AppException(ExceptionType.Auth, AuthFailType.InvalidToken, ex);

        }
    }

    // public static string GenerateJwtToken(Data.UserIdentity user)
    // {
    //     // Secret key for signing the token (use a secure key in production)
    //     // XX var secretKey = "your-very-secure-secret-key";
    //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    //     var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //     // Define claims (e.g., user information)
    //     var claims = new[]
    //     {
    //       //  new Claim("uid", player.UserName),
    //        new Claim("uid", user.UserName)
    //     };

    //     // Create the token
    //     var tokenDescriptor = new JwtSecurityToken(
    //         issuer: "CWGServer",
    //         audience: "CWGplayer",
    //         claims: claims,
    //         expires: DateTime.UtcNow.AddHours(12),
    //         signingCredentials: credentials
    //     );

    //     // Serialize the token
    //     return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    // }

}

