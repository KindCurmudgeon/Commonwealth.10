using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Data;

namespace Authorization;

public static class PasswordCrypto
{
    const int keySize = 32;
    const int iterations = 100000;

    public static string HashPassword(string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(keySize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            HashAlgorithmName.SHA512,
            keySize);

        return Convert.ToHexString(hash);
    }
    public static bool VerifyPassword(string password, string hash, byte[] salt)
    {
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA512, keySize);

        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
    }
}
public static class Token
{
       public static string GenerateJwtToken(UserProfile profile, IConfiguration config)
    {
        // Secret key for signing the token (use a secure key in production)
        // XX var secretKey = "your-very-secure-secret-key";
        string authKey = config["AuthKey"] ?? string.Empty;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Define claims (e.g., user information)
        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.NameId, profile.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, profile.UserName),
            new Claim(JwtRegisteredClaimNames.Email, profile.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.FamilyName, profile.FamilyName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.GivenName, profile.GivenName ?? string.Empty),

        };
        if (profile.IsAdministrator) claims.Add(new Claim(ClaimTypes.Role,"Admin"));
        if (profile.IsDeveloper) claims.Add(new Claim(ClaimTypes.Role,"Developer"));

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