using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.UnitTests.Infrastructure;

public sealed class DependencyInjectionTests
{
    [Theory]
    [InlineData(typeof(ApplicationDbContext))]
    [InlineData(typeof(IClientProfileRepository))]
    [InlineData(typeof(IFreelancerProfileRepository))]
    [InlineData(typeof(ISkillRepository))]
    [InlineData(typeof(IUnitOfWork))]
    public void AddUserProfilesInfrastructure_RegistersScopedService(Type serviceType)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = "Host=localhost" })
            .Build();
        var services = new ServiceCollection();

        services.AddUserProfilesInfrastructure(configuration);

        services.Should().Contain(d => d.ServiceType == serviceType && d.Lifetime == ServiceLifetime.Scoped);
    }
}
