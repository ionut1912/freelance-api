using Freelance.Face.Infrastructure.Services;

namespace Freelance.Face.UnitTests.Infrastructure;

/// <summary>In the same collection as the FaceService tests, one of which counts temporary files.</summary>
[Collection("FaceService")]
public sealed class TemporaryImageFileTests
{
    [Theory]
    [InlineData(new byte[] { 0xFF, 0xD8, 0xFF, 0x00 }, ".jpg")]
    [InlineData(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, ".png")]
    [InlineData(new byte[] { 0x42, 0x4D, 0x00 }, ".bmp")]
    [InlineData(new byte[] { 1, 2, 3 }, ".jpg")]
    [InlineData(new byte[0], ".jpg")]
    public void Constructor_WritesTheBytesWithTheDetectedExtension(byte[] bytes, string extension)
    {
        using var file = new TemporaryImageFile(bytes);

        file.Path.Should().EndWith(extension);
        File.ReadAllBytes(file.Path).Should().Equal(bytes);
    }

    [Fact]
    public void Dispose_DeletesTheFile()
    {
        var file = new TemporaryImageFile([1]);

        file.Dispose();

        File.Exists(file.Path).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Null_Throws() =>
        FluentActions.Invoking(() => new TemporaryImageFile(null!)).Should().Throw<ArgumentNullException>();
}
