using Frelance.Application;
using Frelance.Infrastructure;
using Frelance.Web.Handlers;
using Frelance.Web.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
app.Run();