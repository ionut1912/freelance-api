using System.Reflection;
using FluentValidation;
using Frelance.API.Frelance.Application.Mapings;
using Frelance.Application.Behaviors;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Frelance.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cf =>
        {
            cf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cf.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        MapingConfig.Configure();
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(config);

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}