using System.Reflection;
using System.Text;
using Frelance.Application.Repositories;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Frelance.Infrastructure.Mappings;
using Frelance.Infrastructure.Services;
using Frelance.Infrastructure.Settings;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Frelance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        MappingConfig.Configure();
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(config);
        services.AddDbContext<FrelanceDbContext>();
        services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ITimeLogRepository, TimeLogRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddIdentityCore<Users>(opt => opt.User.RequireUniqueEmail = true)
            .AddRoles<Roles>()
            .AddEntityFrameworkStores<FrelanceDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<Users>>(TokenOptions.DefaultProvider);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(configuration["JWTSettings:TokenKey"]))
                };
            });
        services.AddScoped<TokenService>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IUserAccessor, UserAccessor>();
        return services;
    }
}