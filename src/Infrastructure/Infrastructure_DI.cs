using Application.Contracts;
using Infrastructure.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class Infrastructure_DI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // services.add;
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Local-Cache");
        });
        services.AddSingleton<ICacheServices, CacheServices>();
        services.AddDistributedMemoryCache();
        return services;
    }
}