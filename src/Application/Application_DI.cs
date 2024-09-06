using Application.Behaviors;
using Domain.Primitives;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;
public static class Application_DI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CurrentUser>();
        services.AddMediatR(option =>
        {
            option.RegisterServicesFromAssembly(typeof(Application_DI).Assembly);
            option.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}