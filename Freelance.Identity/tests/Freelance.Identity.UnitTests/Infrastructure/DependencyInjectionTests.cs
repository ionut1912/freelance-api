using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Infrastructure;
using Freelance.Identity.Infrastructure.Persistance;
using Freelance.Identity.Infrastructure.Persistance.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interfaces;

namespace Freelance.Identity.UnitTests.Infrastructure;

public sealed class DependencyInjectionTests
{
    private static ServiceCollection Register()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = "Host=localhost" })
            .Build();
        var services = new ServiceCollection();
        services.AddIdentityInfrastructure(configuration);
        return services;
    }

    [Theory]
    [InlineData(typeof(ApplicationDbContext))]
    [InlineData(typeof(IAccountRepository))]
    [InlineData(typeof(IUnitOfWork))]
    [InlineData(typeof(IPasswordService))]
    [InlineData(typeof(IJwtTokenService))]
    [InlineData(typeof(TimeProvider))]
    public void AddIdentityInfrastructure_RegistersService(Type serviceType) =>
        Register().Should().Contain(d => d.ServiceType == serviceType);

    [Fact]
    public void AddIdentityInfrastructure_RegistersStatelessServicesAsSingletons()
    {
        var services = Register();

        services.Should().ContainSingle(d => d.ServiceType == typeof(IPasswordService))
            .Which.Should().BeEquivalentTo(new { Lifetime = ServiceLifetime.Singleton, ImplementationType = typeof(PasswordService) });
        services.Should().ContainSingle(d => d.ServiceType == typeof(IJwtTokenService))
            .Which.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }
}
