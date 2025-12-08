using FluentValidation;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Mediator;


namespace Freelance.Identity.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(CreateAccountCommand).Assembly;
        services.AddMediator(applicationAssembly);
        services.AddValidatorsFromAssembly(applicationAssembly);
        return services;
    }
}
