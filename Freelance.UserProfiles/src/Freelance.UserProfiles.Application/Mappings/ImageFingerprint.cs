using System.Security.Cryptography;
using System.Text;

namespace Freelance.UserProfiles.Application.Mappings;

/// <summary>A short, stable identifier of a profile image, used to match a verification result to its request.</summary>
public static class ImageFingerprint
{
    public static string Of(string image) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(image)));
}
