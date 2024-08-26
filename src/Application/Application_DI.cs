using Domain.Primitives;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Application;
public static class Application_DI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CurrentUser>();
        services.AddMediatR(option =>
        {
            option.RegisterServicesFromAssembly(typeof(Application_DI).Assembly);
        });
        return services;
    }
}