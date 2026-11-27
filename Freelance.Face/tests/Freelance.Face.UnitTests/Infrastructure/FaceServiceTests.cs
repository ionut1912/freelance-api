using System.Buffers.Binary;
using Freelance.Face.Infrastructure.Services;

namespace Freelance.Face.UnitTests.Infrastructure;

/// <summary>Runs the real dlib models from Freelance.Face/models (native DlibDotNet libraries required).</summary>
public sealed class FaceServiceTests : IDisposable
{
    private readonly FaceService _service = new();

    /// <summary>Builds an uncompressed 24-bit BMP filled with a single colour (no face in it).</summary>
    private static byte[] SolidBitmap(int width, int height, byte gray)
    {
        var rowSize = (width * 3 + 3) & ~3;
        var pixels = rowSize * height;
        var bmp = new byte[54 + pixels];
        bmp[0] = (byte)'B';
        bmp[1] = (byte)'M';
        BinaryPrimitives.WriteInt32LittleEndian(bmp.AsSpan(2), bmp.Length);
        BinaryPrimitives.WriteInt32LittleEndian(bmp.AsSpan(10), 54);
        BinaryPrimitives.WriteInt32LittleEndian(bmp.AsSpan(14), 40);
        BinaryPrimitives.WriteInt32LittleEndian(bmp.AsSpan(18), width);
        BinaryPrimitives.WriteInt32LittleEndian(bmp.AsSpan(22), height);
        BinaryPrimitives.WriteInt16LittleEndian(bmp.AsSpan(26), 1);
        BinaryPrimitives.WriteInt16LittleEndian(bmp.AsSpan(28), 24);
        BinaryPrimitives.WriteInt32LittleEndian(bmp.AsSpan(34), pixels);
        bmp.AsSpan(54).Fill(gray);
        return bmp;
    }

    [Fact]
    public void TryGetFaceDescriptor_ImageWithoutFace_ReturnsFalse()
    {
        _service.TryGetFaceDescriptor(SolidBitmap(64, 64, 200), out var descriptor).Should().BeFalse();
        descriptor.Should().BeNull();
    }

    [Theory]
    [InlineData(new byte[] { 0xFF, 0xD8, 0xFF, 0x00 })]
    [InlineData(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00 })]
    [InlineData(new byte[] { 1, 2, 3 })]
    public void TryGetFaceDescriptor_CorruptImage_ReturnsFalse(byte[] bytes) =>
        _service.TryGetFaceDescriptor(bytes, out _).Should().BeFalse();

    [Fact]
    public void TryGetFaceDescriptor_Null_Throws() =>
        _service.Invoking(s => s.TryGetFaceDescriptor(null!, out _)).Should().Throw<ArgumentNullException>();

    [Fact]
    public void TryGetFaceDescriptor_CleansUpTemporaryFiles()
    {
        var before = Directory.GetFiles(Path.GetTempPath()).Length;

        for (var i = 0; i < 3; i++) _service.TryGetFaceDescriptor(SolidBitmap(16, 16, 10), out _);

        Directory.GetFiles(Path.GetTempPath()).Length.Should().BeLessThanOrEqualTo(before);
    }

    [Fact]
    public void Distance_IsEuclidean_OverTheCommonLength()
    {
        _service.Distance([0f, 0f], [3f, 4f]).Should().BeApproximately(5, 1e-6);
        _service.Distance([1f, 1f, 9f], [1f, 1f]).Should().Be(0);
    }

    [Fact]
    public void Distance_NullVector_Throws()
    {
        _service.Invoking(s => s.Distance(null!, [1f])).Should().Throw<ArgumentNullException>();
        _service.Invoking(s => s.Distance([1f], null!)).Should().Throw<ArgumentNullException>();
    }

    public void Dispose() => _service.Dispose();
}
