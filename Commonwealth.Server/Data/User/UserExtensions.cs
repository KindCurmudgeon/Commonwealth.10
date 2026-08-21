using System.Security.Cryptography;
using System.Text;
using Commonwealth.Server.Utilities;

namespace Commonwealth.Server.Data;

public partial class User
{
    public static async Task<bool> IsExisting(string? userName, BlobService blobService)
    {
        if (userName is null) return false;
        bool exists = await blobService.IsExisting(BlobPath(userName));
        return exists;
    }
}

public static class PasswordCrypto
{
    const int keySize = 32;
    const int iterations = 100000;
    //  HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;


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

public static class UserExtensions
{
    // public static UserIdentity CreateUserIdentity(this User user)
    // {
    //     return new UserIdentity()
    //     {
    //         UserName = user.UserName,
    //         Email = user.Email
    //     };
    // }
    public static void RemoveAllGames(this User user, string gameName)
    {
        user.NationIdentities.RemoveAll(i => i.GameName == gameName);
        //  user.GamemasterGames.RemoveAll(g => g == gameName);
    }
    // public static List<string> GetGames(this User user)
    // {
    //     List<string> games = [];
    //     foreach (NationIdentity identity in user.ParticipantGames) { games.AddIfNotDuplicate(identity.GameName); }
    //     foreach (string gameName in user.GamemasterGames) games.Add(gameName);
    //     return games;
    // }
}
