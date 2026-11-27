using System.Reflection;
using Freelance.Face.Infrastructure.Services;

namespace Freelance.Face.UnitTests.Infrastructure;

/// <summary>Covers the private model lookup of <see cref="FaceService"/> (bin/models, then parent folders).</summary>
public sealed class FaceServiceModelResolutionTests
{
    private static readonly MethodInfo ResolveModelPath =
        typeof(FaceService).GetMethod("ResolveModelPath", BindingFlags.NonPublic | BindingFlags.Static)!;

    private static string Resolve(string fileName) => (string)ResolveModelPath.Invoke(null, [fileName])!;

    [Theory]
    [InlineData("shape_predictor_5_face_landmarks.dat")]
    [InlineData("dlib_face_recognition_resnet_model_v1.dat")]
    public void ResolveModelPath_FindsTheRepositoryModels(string fileName)
    {
        var path = Resolve(fileName);

        File.Exists(path).Should().BeTrue();
        path.Should().EndWith(Path.Combine("models", fileName));
    }

    [Fact]
    public void ResolveModelPath_PrefersTheModelsFolderNextToTheApplication()
    {
        var local = Path.Combine(AppContext.BaseDirectory, "models", $"{Guid.NewGuid():N}.dat");
        Directory.CreateDirectory(Path.GetDirectoryName(local)!);
        File.WriteAllBytes(local, []);
        try
        {
            Resolve(Path.GetFileName(local)).Should().Be(local);
        }
        finally
        {
            File.Delete(local);
        }
    }

    [Fact]
    public void ResolveModelPath_MissingModel_ThrowsFileNotFound() =>
        FluentActions.Invoking(() => Resolve($"{Guid.NewGuid():N}.dat"))
            .Should().Throw<TargetInvocationException>().WithInnerException<FileNotFoundException>();
}
