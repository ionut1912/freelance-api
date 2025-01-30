using Frelance.Application;
using Frelance.Infrastructure;
using Frelance.Web.Handlers;
using Frelance.Web.Modules;
using Microsoft.OpenApi.Models;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

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
builder.Services.AddInfrastructure(configuration);
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

// Fetch secrets from Azure Key Vault using Managed Identity
var keyVaultUrl = configuration["AzureKeyVault:VaultUrl"];
var connectionStringSecretName = configuration["AzureKeyVault:ConnectionStringSecretName"];
var jwtTokenSecretName = configuration["AzureKeyVault:JWTTokenSecretName"];

if (!string.IsNullOrEmpty(keyVaultUrl) && !string.IsNullOrEmpty(connectionStringSecretName) && !string.IsNullOrEmpty(jwtTokenSecretName))
{
    var credential = new ManagedIdentityCredential();
    var client = new SecretClient(new Uri(keyVaultUrl), credential);
    
    int maxRetries = 5;
    int delay = 3000; // Start with 3 seconds

    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            var connectionStringSecret = client.GetSecret(connectionStringSecretName);
            var jwtTokenSecret = client.GetSecret(jwtTokenSecretName);

            builder.Configuration["DatabaseSettings:ConnectionString"] = connectionStringSecret.Value.Value;
            builder.Configuration["JWTTokenKey"] = jwtTokenSecret.Value.Value;
            Console.WriteLine("✅ Successfully retrieved secrets from Azure Key Vault.");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠ Attempt {i + 1} - Failed to retrieve secrets. Retrying in {delay / 1000} seconds...");
            Task.Delay(delay).Wait();
            delay *= 2; // Exponential backoff (3s → 6s → 12s → 24s → 48s)
        }
    }
}
else
{
    Console.WriteLine("⚠ Warning: Azure Key Vault configuration is missing from appsettings.json. Using local secrets.");
}

app.Run();
