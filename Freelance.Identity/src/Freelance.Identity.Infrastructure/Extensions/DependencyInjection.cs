using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Infrastructure.Persistance;
using Freelance.Identity.Infrastructure.Persistance.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interfaces;
using Shared.Infra.Extensions;
using Shared.Infra.Services;

namespace Freelance.Identity.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDatabaseContext<ApplicationDbContext>(configuration);

        // Unit of Work
        services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();

        // Repositories
        services.AddScoped<IAccountRepository, AccountRepository>();

        // Infrastructure Services
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }

    public static IApplicationBuilder MigrateIdentityDatabase(this IApplicationBuilder app)
    {
        app.ApplicationServices.MigrateDatabase<ApplicationDbContext>();
        return app;
    }
}
