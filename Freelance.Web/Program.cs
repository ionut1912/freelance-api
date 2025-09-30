using Freelance.Application;
using Freelance.Infrastructure;
using Freelance.Infrastructure.Hubs;
using Freelance.Web.Handlers;
using Freelance.Web.Modules;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

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
        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() }
    });
});

builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
});
builder.Services.AddControllers();

const string frontendOrigin = "_frontendOrigin";
var feEnv = Environment.GetEnvironmentVariable("FRONTEND_BASE_URL");
builder.Services.AddCors(options =>
{
    options.AddPolicy(frontendOrigin, policy =>
        policy
            .WithOrigins(
                "http://localhost:3000",
                "https://localhost:3000",
                feEnv ?? "https://frontend.example"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    );
});

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();

app.UseCors(frontendOrigin);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<CaptureHub>("/hubs/capture");

app.AddProjectsEndpoints();
app.AddTasksEndpoints();
app.AddTimeLogsEndpoints();
app.AddUserEndpoints();
app.AddSkillsEndpoint();
app.AddClientProfileEndpoints();
app.AddUserProfileEndpoints();
app.AddFreelancerProfilesEndpoints();
app.AddReviewsEndpoints();
app.AddContractEndpoints();
app.AddInvoicesEndpoints();
app.AddProposalEndpoints();
app.AddFaceEndpoints();
app.AddCameraEndpoints();

app.Run();
