using System.Text;
using FluentValidation;
using Freelance.Shared.Application.Behaviours;
using Freelance.Shared.Domain.Interfaces;
using Freelance.UserProfiles.Api.Handlers;
using Freelance.UserProfiles.Api.Modules;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Freelance.UserProfiles.Infrastructure.Persistance.Repository;
using Freelancer.UserProfiles.Application.Mappings;
using Freelancer.UserProfiles.Application.Mediatr;
using Freelancer.UserProfiles.Application.Validators;
using Frelance.Shared.Infrastructure.Extensions;
using Frelance.Shared.Infrastructure.Services;
using Frelance.Shared.Infrastructure.Settings;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// ---------- Database ----------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------- MediatR ----------
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(MediatrAssemblyReference).Assembly);
});

// ---------- AutoMapper ----------
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

// ---------- FluentValidation ----------
builder.Services.AddValidatorsFromAssembly(typeof(ValidationAssemblyReference).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// ---------- JWT ----------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

// ---------- Dependency Injection ----------
builder.Services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();
builder.Services.AddScoped<IFreelancerProfileRepository, FreelancerProfileService>();
builder.Services.AddScoped<IClientProfileRepository, ClientProfileService>();

// Add ExceptionHandler
builder.Services.AddSingleton<IExceptionHandler, ExceptionHandler>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = jwtSettings["Key"];
    if (string.IsNullOrEmpty(key))
        throw new InvalidOperationException("JWT Key is missing in configuration.");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

// ---------- Authorization ----------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("FreelancerOnly", policy => policy.RequireRole("Freelancer"));
    options.AddPolicy("ClientOnly", policy => policy.RequireRole("Client"));
});

// ---------- Swagger ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Freelance Identity API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

// ---------- Controllers ----------
builder.Services.AddControllers();

var app = builder.Build();

// ---------- Apply Pending Migrations ----------
app.Services.MigrateDatabase<ApplicationDbContext>();

// ---------- Global Exception Handling ----------
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var exceptionHandler = context.RequestServices.GetRequiredService<IExceptionHandler>();
        await exceptionHandler.TryHandleAsync(context, ex, CancellationToken.None);
    }
});

// ---------- Swagger ----------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ---------- Middleware ----------
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// ---------- Endpoints ----------
app.AddClientProfileEndpoints();
app.AddFreelancerProfileEndpoints();

app.Run();