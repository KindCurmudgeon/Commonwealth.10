using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core.Cryptography;
using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;
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
    public static async Task<User> ValidateUserAsync(RequestBase request, BlobService blobService)
    {
        // try
        // {
        string userName = ExtractUserNameFromToken(request.Token);
        User user = await User.RetrieveAsync(userName, blobService);
        return user;
        //}
        //catch (Exception ex) { ex.FailedItem = $"User '{request.ExtractUserNameFromToken}throw new AppException(Message.UnrecogonizedUser(), ex); }

    }



    private static string ExtractUserNameFromToken(string? token)
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

            // Create the token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Validate the token
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            Claim? claim = principal.Claims.FirstOrDefault(c => c.Type == "uid");
            string userName = (claim?.Value.ToString()) ?? throw new Exception("Null UserName");
            return userName;
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

    public static string GenerateJwtToken(User user)
    {
        // Secret key for signing the token (use a secure key in production)
        // XX var secretKey = "your-very-secure-secret-key";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Define claims (e.g., user information)
        var claims = new[]
        {
          //  new Claim("uid", player.UserName),
           new Claim("uid", user.UserName)
        };

        // Create the token
        var tokenDescriptor = new JwtSecurityToken(
            issuer: "CWGServer",
            audience: "CWGplayer",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: credentials
        );

        // Serialize the token
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

}

