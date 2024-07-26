using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class Persistence_DI
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // services.add;
        services.AddDbContext<VitomDBContext>(option =>
            {
                option.UseNpgsql(configuration.GetConnectionString("Default"), builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
                option.EnableSensitiveDataLogging();
                option.EnableDetailedErrors();
            });
        return services;
    }
}