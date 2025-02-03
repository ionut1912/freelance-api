using System.Reflection;
using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Frelance.Application.Repositories;
using Frelance.Infrastructure.Context;
using Frelance.Infrastructure.Entities;
using Frelance.Infrastructure.Extensions;
using Frelance.Infrastructure.Mappings;
using Frelance.Infrastructure.Services;
using Frelance.Infrastructure.Settings;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger("Startup");
        var databaseSettings = new DatabaseSettings();
        configuration.GetSection("DatabaseSettings").Bind(databaseSettings);
        var connectionStringSecretName = configuration["AzureKeyVault__ConnectionStringSecretName"];
        var jwtSecretName = configuration["AzureKeyVault__JWTTokenSecretName"];

        try
        {
            if (string.IsNullOrEmpty(connectionStringSecretName))
            {
                throw new InvalidOperationException("AzureKeyVault__ConnectionStringSecretName is not configured.");
            }
            
            if (string.IsNullOrEmpty(jwtSecretName))
            {
                throw new InvalidOperationException("AzureKeyVault__JWTTokenSecretName is not configured.");
            }

            var connectionString = configuration.GetSecret(connectionStringSecretName);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"The secret '{connectionStringSecretName}' returned an empty connection string.");
            }

            logger.LogInformation("Connection string retrieved successfully. (Value not displayed for security)");
            databaseSettings.ConnectionString = connectionString;

            var jwtTokenKey = configuration.GetSecret(jwtSecretName);
            if (string.IsNullOrEmpty(jwtTokenKey))
            {
                throw new InvalidOperationException($"The secret '{jwtSecretName}' returned an empty JWT token key.");
            }

            logger.LogInformation("JWT token key retrieved successfully. (Value not displayed for security)");

            // Ensure Database Connection is Set Before UseSqlServer
            services.AddDbContext<FrelanceDbContext>((serviceProvider, options) =>
            {
                var dbSettings = serviceProvider.GetRequiredService<IConfiguration>()
                                                .GetSection("DatabaseSettings")
                                                .Get<DatabaseSettings>();

                if (string.IsNullOrEmpty(dbSettings?.ConnectionString))
                {
                    throw new InvalidOperationException("Database connection string is missing or not set.");
                }

                options.UseSqlServer(dbSettings.ConnectionString);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenKey))
                    };
                });

            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITimeLogRepository, TimeLogRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddIdentityCore<Users>(opt => opt.User.RequireUniqueEmail = true)
                .AddRoles<Roles>()
                .AddEntityFrameworkStores<FrelanceDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<Users>>(TokenOptions.DefaultProvider);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ClientRole", policy => policy.RequireRole("Client"));
                options.AddPolicy("FrelancerRole", policy => policy.RequireRole("Frelancer"));
            });

            services.AddScoped<TokenService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserAccessor, UserAccessor>();

            logger.LogInformation("Infrastructure services configured successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error configuring infrastructure: {ex.Message}");
            throw;
        }

        return services;
    }
}
