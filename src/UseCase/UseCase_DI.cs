using Microsoft.Extensions.DependencyInjection;

namespace UseCase;
public static class UseCase_DI
{
    public static IServiceCollection AddUseCase(this IServiceCollection services)
    {
        services.AddMediatR(option =>
        {
            option.RegisterServicesFromAssembly(typeof(UseCase_DI).Assembly);
        });
        return services;
    }
}