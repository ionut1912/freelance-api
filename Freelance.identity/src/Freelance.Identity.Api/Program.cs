using System.Diagnostics;
using System.Text;
using FluentValidation;
using Freelance.Identity.Api.Handlers;
using Freelance.Identity.Api.Modules;
using Freelance.Identity.Application.Mappings;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Validators;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Infrastructure.Persistance;
using Freelance.Identity.Infrastructure.Persistance.Repositories;
using Freelance.Shared.Application.Behaviours;
using Freelance.Shared.Domain.Interfaces;
using Freelance.Shared.Infrastructure.Extensions;
using Freelance.Shared.Infrastructure.Services;
using Freelance.Shared.Infrastructure.Settings;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// ------------------------ Configuration ------------------------
var configuration = builder.Configuration;
var jwtSettings = configuration.GetSection("JwtSettings");

// OTEL endpoint: prefer env var OTEL_EXPORTER_OTLP_ENDPOINT, fallback to alloy (internal docker service)
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://alloy:4317";
var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-Identity";
var environmentName = builder.Environment.EnvironmentName ?? "Development";

// ------------------------ Resource (service metadata) ------------------------
var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService(serviceName: serviceName, serviceVersion: "1.0.0")
    .AddAttributes(new[]
    {
        new KeyValuePair<string, object>("deployment.environment", environmentName),
        new KeyValuePair<string, object>("service.namespace", "freelance"),
    });

// ------------------------ Database ------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ------------------------ MediatR / AutoMapper / FluentValidation ------------------------
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateAccountCommand).Assembly));
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CreateAccountCommandValidator).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// ------------------------ JWT ------------------------
builder.Services.Configure<JwtSettings>(jwtSettings);

// ------------------------ DI ------------------------
builder.Services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// Add ExceptionHandler
builder.Services.AddSingleton<IExceptionHandler, ExceptionHandler>();

// ------------------------ Authentication & Authorization ------------------------
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("FreelancerOnly", policy => policy.RequireRole("Freelancer"));
    options.AddPolicy("ClientOnly", policy => policy.RequireRole("Client"));
});


builder.Logging.ClearProviders();
builder.Logging.AddConsole(); 
builder.Logging.AddOpenTelemetry(options =>
{
    options.SetResourceBuilder(resourceBuilder);
    options.IncludeScopes = true;
    options.IncludeFormattedMessage = true;
    options.ParseStateValues = true;
    options.AddOtlpExporter(otlpOptions =>
    {
        otlpOptions.Endpoint = new Uri(otelEndpoint);
    });
});

// ---- Tracing & Metrics ----
builder.Services.AddOpenTelemetry()
    .ConfigureResource(rb => rb.AddService(serviceName: serviceName, serviceVersion: "1.0.0"))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation(opts =>
            {
                opts.RecordException = true;
            })
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation(options =>
            {
                // Enrich spans with SQL text
                options.EnrichWithIDbCommand = (activity, command) =>
                {
                    activity.SetTag("db.statement", command.CommandText);
                };
            })
            .AddSource("Freelance.Identity")
            .AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri(otelEndpoint);
            });
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()   // ✅ metrics only
            .AddProcessInstrumentation()
            .AddEventCountersInstrumentation(c =>
            {
                c.AddEventSources("Microsoft.AspNetCore.Hosting");
            })
            .AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri(otelEndpoint);
            });
    });

// ------------------------ Health, Swagger, Controllers ------------------------
builder.Services.AddHealthChecks();
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
builder.Services.AddControllers();

var app = builder.Build();
app.Services.MigrateDatabase<ApplicationDbContext>();

// Global exception handler middleware
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Unhandled exception caught in global middleware");

        var exceptionHandler = context.RequestServices.GetRequiredService<IExceptionHandler>();
        await exceptionHandler.TryHandleAsync(context, ex, CancellationToken.None);
    }
});

// Request duration + route tagging middleware
app.Use(async (context, next) =>
{
    var sw = Stopwatch.StartNew();

    if (Activity.Current == null)
    {
        var activity = new Activity("http.server");
        activity.Start();
    }

    try
    {
        await next();
    }
    finally
    {
        sw.Stop();
        Activity.Current?.SetTag("http.request_duration_ms", sw.ElapsedMilliseconds);
        if (context.GetEndpoint() is Endpoint endpoint)
        {
            var routePattern = endpoint.Metadata.GetMetadata<RouteNameMetadata>()?.RouteName
                               ?? endpoint.DisplayName;
            if (!string.IsNullOrEmpty(routePattern))
            {
                Activity.Current?.SetTag("http.route", routePattern);
            }
        }
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms",
            context.Request.Method,
            context.Request.Path,
            context.Response?.StatusCode,
            sw.ElapsedMilliseconds);
    }
});

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapGet("/metrics", async context =>
{
    context.Response.StatusCode = 204;
    await context.Response.CompleteAsync();
});
app.AddUserEndpoints();

app.Run();
