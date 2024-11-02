using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.DataGenerator;
using Type = Domain.Entities.Type;

namespace Persistence;

public static class VitomDBContextSeed
{
    public static async Task Seed(this VitomDBContext context)
    {
        if (context.Set<User>().AsNoTracking().FirstOrDefault() is not null)
        {
            return;
        }
        try
        {
            await TrySeedAsync(context);
        }
        catch (Exception ex)
        {
            Console.Write(ex);
            throw;
        }
    }
    private static async Task TrySeedAsync(VitomDBContext context)
    {
        List<Task> tasks = [];
        Type[] types = TypeGenerator.Generate();
        Software[] softwares = SoftwareGenerator.Generate();
        tasks.Add(context.AddRangeAsync(types));
        tasks.Add(context.AddRangeAsync(softwares));
        await Task.WhenAll(tasks);
        await ((Application.Contracts.IVitomDbContext)context).SaveChangesAsync();
    }
}