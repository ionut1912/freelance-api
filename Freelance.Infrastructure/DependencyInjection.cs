using System.Reflection;
using System.Text;
using Freelance.Application.Repositories;
using Freelance.Application.Repositories.ML;
using Freelance.Infrastructure.Context;
using Freelance.Infrastructure.Entities;
using Freelance.Infrastructure.Mappings;
using Freelance.Infrastructure.Services;
using Freelance.Infrastructure.Services.ML;
using Freelance.Infrastructure.Settings;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Freelance.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        MappingConfig.Configure();
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(config);
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger("Startup");
        var databaseSettings = new DatabaseSettings();
        configuration.GetSection("DatabaseSettings").Bind(databaseSettings);

        try
        {
            var connectionString = configuration["ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException(
                    $"The secret '{nameof(connectionString)}' returned an empty connection string.");

            logger.LogInformation("Connection string retrieved successfully. (Value not displayed for security)");
            databaseSettings.ConnectionString = connectionString;

            var jwtTokenKey = configuration["JwtTokenKey"];
            if (string.IsNullOrEmpty(jwtTokenKey))
                throw new InvalidOperationException(
                    $"The secret '{nameof(jwtTokenKey)}' returned an empty JWT token key.");

            logger.LogInformation("JWT token key retrieved successfully. (Value not displayed for security)");

            services.AddDbContext<FreelanceDbContext>(options =>
                options.UseSqlServer(databaseSettings.ConnectionString));
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
            services.AddScoped<ISkillsRepository, SkillRepository>();
            services.AddScoped<IClientProfileRepository, ClientProfileRepository>();
            services.AddScoped<IFreelancerProfileRepository, FreelancerProfileRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IProposalRepository, ProposalRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IFaceComparisionService, FaceRecognitionFaceComparisonService>();

            services.AddIdentityCore<Users>(opt => opt.User.RequireUniqueEmail = true)
                .AddRoles<Roles>()
                .AddEntityFrameworkStores<FreelanceDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<Users>>(TokenOptions.DefaultProvider);

            services.AddAuthorizationBuilder()
                .AddPolicy("ClientRole", policy => policy.RequireRole("Client"))
                .AddPolicy("FreelancerRole", policy => policy.RequireRole("Freelancer"));

            services.AddScoped<TokenService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserAccessor, UserAccessor>();

            logger.LogInformation("Infrastructure services configured successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error configuring infrastructure: {ErrorMessage}", ex.Message);
            throw;
        }
    }
}