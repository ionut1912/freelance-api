using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Interfaces;
using Freelance.ProjectManagement.Infrastructure.Persistance;
using Freelance.ProjectManagement.Infrastructure.Persistance.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interfaces;
using Shared.Infra.Extensions;

namespace Freelance.ProjectManagement.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        /// <summary>Registers persistence and repositories of the ProjectManagement service.</summary>
        public IServiceCollection AddProjectManagementInfrastructure(IConfiguration configuration) =>
            services
                .AddDatabase<ApplicationDbContext>(configuration)
                .AddRepository<Project, ProjectRepository, IProjectRepository, ApplicationDbContext>()
                .AddRepository<ProjectTask, ProjectTaskRepository, IProjectTaskRepository, ApplicationDbContext>()
                .AddRepository<TimeLog, TimeLogRepository, ITimeLogRepository, ApplicationDbContext>()
                .AddRepos<IUnitOfWork, UnitOfWork>();
    }
}
