namespace Freelance.Face.Infrastructure.Services;

/// <summary>An image written to disk for as long as the instance lives (DlibDotNet only decodes files).</summary>
public sealed class TemporaryImageFile : IDisposable
{
    public TemporaryImageFile(byte[] imageBytes)
    {
        ArgumentNullException.ThrowIfNull(imageBytes);

        Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{Guid.NewGuid():N}{DetectExtension(imageBytes)}");
        File.WriteAllBytes(Path, imageBytes);
    }

    public string Path { get; }

    public void Dispose() => File.Delete(Path);

    private static string DetectExtension(ReadOnlySpan<byte> bytes) => bytes switch
    {
        [0xFF, 0xD8, 0xFF, ..] => ".jpg",
        [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, ..] => ".png",
        [0x42, 0x4D, ..] => ".bmp",
        _ => ".jpg"
    };
}
