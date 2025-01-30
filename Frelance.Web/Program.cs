using Frelance.Application;
using Frelance.Infrastructure;
using Frelance.Web.Handlers;
using Frelance.Web.Modules;
using Microsoft.OpenApi.Models;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid JWT token.\nExample: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<ExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(_ => { });
app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials();
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.AddProjectsEndpoints();
app.AddTasksEndpoints();
app.AddTimeLogsEndpoints();
app.AddUserEndpoints();

// Fetch secrets from Azure Key Vault
var keyVaultUrl = app.Configuration["AzureKeyVault:VaultUrl"];
if (!string.IsNullOrEmpty(keyVaultUrl))
{
    var credential = new ManagedIdentityCredential();
    var client = new SecretClient(new Uri(keyVaultUrl), credential);

    try
    {
        var connectionStringSecretName = app.Configuration["AzureKeyVault:ConnectionStringSecretName"];
        var jwtTokenSecretName = app.Configuration["AzureKeyVault:JWTTokenSecretName"];

        if (!string.IsNullOrEmpty(connectionStringSecretName))
        {
            var connectionStringSecret = client.GetSecret(connectionStringSecretName);
            app.Configuration["DatabaseSettings:ConnectionString"] = connectionStringSecret.Value.Value;
        }
        else
        {
            throw new Exception("ConnectionStringSecretName is missing from Azure Key Vault configuration.");
        }

        if (!string.IsNullOrEmpty(jwtTokenSecretName))
        {
            var jwtTokenSecret = client.GetSecret(jwtTokenSecretName);
            app.Configuration["JWTTokenKey"] = jwtTokenSecret.Value.Value;
        }
        else
        {
            throw new Exception("JWTTokenSecretName is missing from Azure Key Vault configuration.");
        }
    }
    catch (Exception ex)
    {
        throw new Exception("Failed to retrieve secrets from Azure Key Vault", ex);
    }
}
else
{
    throw new Exception("Azure Key Vault configuration is missing.");
}

app.Run();
