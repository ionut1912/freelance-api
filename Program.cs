using Frelance.API.Handlers;
using Frelance.API.Modules;
using Frelance.Application;
using Frelance.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext with the connection string from Configuration
builder.Services.AddDbContext<FrelanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseSettings:ConnectionString")));
builder.Services.AddApplication();
builder.Services.AddExceptionHandler<ExceptionHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.AddProjectsEndpoints();
app.Run();