using System.Security.Cryptography;
using System.Text;

namespace Offices.Utils;

public class HashUtil
{
    public static string HashSHA256(string source)
    {
        var bytes = Encoding.UTF8.GetBytes(source);
        var hashedBytes = SHA256.HashData(bytes);
        return Convert.ToHexString(hashedBytes);
    }
}