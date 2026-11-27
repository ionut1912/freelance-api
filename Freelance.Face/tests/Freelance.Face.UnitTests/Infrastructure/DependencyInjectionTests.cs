using Freelance.Face.Domain.Interfaces;
using Freelance.Face.Infrastructure;
using Freelance.Face.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Freelance.Face.UnitTests.Infrastructure;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void AddFaceInfrastructure_RegistersFaceServiceAsSingleton() =>
        new ServiceCollection().AddFaceInfrastructure().Should().ContainSingle(d => d.ServiceType == typeof(IFaceService))
            .Which.Should().BeEquivalentTo(new { Lifetime = ServiceLifetime.Singleton, ImplementationType = typeof(FaceService) });
}
