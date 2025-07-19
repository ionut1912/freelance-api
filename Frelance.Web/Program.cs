using Frelance.Application;
using Frelance.Infrastructure;
using Frelance.Web.Handlers;
using Frelance.Web.Modules;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region Swagger
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
        Description =
            "Enter 'Bearer' [space] and then your valid JWT token.\n" +
            "Example: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"
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
#endregion

// 1️⃣  CORS registration with a named policy
const string FrontendOrigin = "_frontendOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendOrigin, policy =>
        policy.WithOrigins("http://localhost:3000")   // exact origin
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());                  // only if you need cookies/Auth
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

// 2️⃣  Apply the policy early (before auth)
app.UseCors(FrontendOrigin);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// 3️⃣  Optional: make every endpoint group require this policy
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

app.Run();
