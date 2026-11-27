using Freelance.Face.Infrastructure.Services;

namespace Freelance.Face.UnitTests.Infrastructure;

/// <summary>Model lookup order: bin/models first, then parent folders.</summary>
public sealed class DlibModelLocatorTests
{
    [Theory]
    [InlineData("shape_predictor_5_face_landmarks.dat")]
    [InlineData("dlib_face_recognition_resnet_model_v1.dat")]
    public void Resolve_FindsTheRepositoryModels(string fileName)
    {
        var path = DlibModelLocator.Resolve(fileName);

        File.Exists(path).Should().BeTrue();
        path.Should().EndWith(Path.Combine("models", fileName));
    }

    [Fact]
    public void Resolve_PrefersTheModelsFolderNextToTheApplication()
    {
        var local = Path.Combine(AppContext.BaseDirectory, "models", $"{Guid.NewGuid():N}.dat");
        Directory.CreateDirectory(Path.GetDirectoryName(local)!);
        File.WriteAllBytes(local, []);
        try
        {
            DlibModelLocator.Resolve(Path.GetFileName(local)).Should().Be(local);
        }
        finally
        {
            File.Delete(local);
        }
    }

    [Fact]
    public void Resolve_MissingModel_ThrowsFileNotFound() =>
        FluentActions.Invoking(() => DlibModelLocator.Resolve($"{Guid.NewGuid():N}.dat"))
            .Should().Throw<FileNotFoundException>();
}
