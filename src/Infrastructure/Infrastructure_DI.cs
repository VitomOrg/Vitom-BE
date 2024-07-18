using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class Infrastructure_DI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // services.add;
        services.AddDbContext<VitomDBContext>(option =>
            {
                option.UseNpgsql(configuration.GetConnectionString("Default"));
                option.EnableSensitiveDataLogging();
                option.EnableDetailedErrors();
            });
        return services;
    }
}