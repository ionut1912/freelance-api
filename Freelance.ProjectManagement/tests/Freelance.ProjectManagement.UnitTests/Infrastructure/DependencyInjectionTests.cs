using Freelance.ProjectManagement.Domain.Interfaces;
using Freelance.ProjectManagement.Infrastructure;
using Freelance.ProjectManagement.Infrastructure.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.UnitTests.Infrastructure;

public sealed class DependencyInjectionTests
{
    [Theory]
    [InlineData(typeof(ApplicationDbContext))]
    [InlineData(typeof(IProjectRepository))]
    [InlineData(typeof(IProjectTaskRepository))]
    [InlineData(typeof(ITimeLogRepository))]
    [InlineData(typeof(IUnitOfWork))]
    public void AddProjectManagementInfrastructure_RegistersScopedService(Type serviceType)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = "Host=localhost" })
            .Build();
        var services = new ServiceCollection();

        services.AddProjectManagementInfrastructure(configuration);

        services.Should().Contain(d => d.ServiceType == serviceType && d.Lifetime == ServiceLifetime.Scoped);
    }
}
