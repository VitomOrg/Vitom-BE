using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class HostingExtension
{
    public static IHost MigrateDatabase<TContext>(this IHost host, Func<TContext, IServiceProvider, Task> seeder)
            where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        // var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetRequiredService<TContext>();

        try
        {
            // context?.Database.EnsureDeleted();
            context?.Database.Migrate();
            // logger.LogInformation("Migrated successfully");
            seeder(context!, services).Wait();
        }
        catch (Exception ex)
        {
            // logger.LogError(ex, "An error occured while migrating database!");
            Console.WriteLine(ex);
        }

        return host;
    }
}