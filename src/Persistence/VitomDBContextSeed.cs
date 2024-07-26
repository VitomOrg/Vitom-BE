using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.DataGenerator;

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
        User[] users = UserGenerator.Generate();
        tasks.Add(context.AddRangeAsync(users));
        await Task.WhenAll(tasks);
        await context.SaveChangesAsync();
    }
}