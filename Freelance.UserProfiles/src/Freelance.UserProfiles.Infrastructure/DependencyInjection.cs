using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Freelance.UserProfiles.Infrastructure.Persistance.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interfaces;
using Shared.Infra.Extensions;

namespace Freelance.UserProfiles.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        /// <summary>Registers persistence and repositories of the UserProfiles service.</summary>
        public IServiceCollection AddUserProfilesInfrastructure(IConfiguration configuration) =>
            services
                .AddDatabase<ApplicationDbContext>(configuration)
                .AddRepository<ClientProfile, ClientProfileService, IClientProfileRepository, ApplicationDbContext>()
                .AddRepository<FreelancerProfile, FreelancerProfileService, IFreelancerProfileRepository, ApplicationDbContext>()
                .AddRepository<Skill, SkillRepository, ISkillRepository, ApplicationDbContext>()
                .AddRepos<IUnitOfWork, UnitOfWork>();
    }
}
