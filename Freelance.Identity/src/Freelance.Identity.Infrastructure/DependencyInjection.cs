using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Infrastructure.Persistance;
using Freelance.Identity.Infrastructure.Persistance.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Domain.Interfaces;
using Shared.Infra.Extensions;

namespace Freelance.Identity.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        /// <summary>Registers persistence, repositories and security services of the Identity service.</summary>
        public IServiceCollection AddIdentityInfrastructure(IConfiguration configuration)
        {
            services.TryAddSingleton(TimeProvider.System);

            return services
                .AddDatabase<ApplicationDbContext>(configuration)
                .AddRepository<Account, AccountRepository, IAccountRepository, ApplicationDbContext>()
                .AddRepos<IUnitOfWork, UnitOfWork>()
                .AddSingleton<IPasswordService, PasswordService>()
                .AddSingleton<IJwtTokenService, JwtTokenService>();
        }
    }
}
