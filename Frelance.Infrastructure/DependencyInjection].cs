using System.Reflection;
using Frelance.Application.Repositories;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Mappings;
using Frelance.Infrastructure.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Frelance.Infrastructure;

public static class DependencyInjection_
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        MappingConfig.Configure();
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(config);
        services.AddDbContext<FrelanceDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DatabaseSettings:ConnectionString")));
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ITimeLogRepository, TimeLogRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        return services;
    }
}