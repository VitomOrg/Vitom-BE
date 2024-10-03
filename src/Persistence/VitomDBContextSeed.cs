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
        User[] users = UserGenerator.Generate();
        Product[] products = ProductGenerator.Generate(users);
        Type[] types = TypeGenerator.Generate();
        Software[] softwares = SoftwareGenerator.Generate();
        Collection[] collections = CollectionGenerator.Generate(users);
        Cart[] carts = CartGenerator.Generate(users);
        Transaction[] transactions = TransactionGenerator.Generate(users);
        Review[] reviews = ReviewGenerator.Generate(products, users);
        UserLibrary[] userLibraries = UserLibraryGenerator.Generate(users, products);
        LikeProduct[] likeProducts = LikeProductGenerator.Generate(users, products);
        LikeCollection[] likeCollections = LikeCollectionGenerator.Generate(collections, users);
        CollectionProduct[] collectionProducts = CollectionProductGenerator.Generate(collections, products);
        CartItem[] cartItems = CartItemGenerator.Generate(carts, products);
        ProductImage[] productImages = ProductImageGenerator.Generate(products);
        ModelMaterial[] modelMaterials = ModelMaterialGenerator.Generate(products);
        ProductSoftware[] productSoftwares = ProductSoftwareGenerator.Generate(products, softwares);
        ProductType[] productTypes = ProductTypeGenerator.Generate(products, types);
        TransactionDetail[] transactionDetails = TransactionDetailGenerator.Generate(transactions, products);
        Blog[] blogs = BlogGenerator.Generate(users);
        BlogImage[] blogImages = BlogImageGenerator.Generate(blogs);
        tasks.Add(context.AddRangeAsync(users));
        tasks.Add(context.AddRangeAsync(products));
        tasks.Add(context.AddRangeAsync(types));
        tasks.Add(context.AddRangeAsync(softwares));
        tasks.Add(context.AddRangeAsync(collections));
        tasks.Add(context.AddRangeAsync(carts));
        tasks.Add(context.AddRangeAsync(transactions));
        tasks.Add(context.AddRangeAsync(reviews));
        tasks.Add(context.AddRangeAsync(userLibraries));
        tasks.Add(context.AddRangeAsync(likeProducts));
        tasks.Add(context.AddRangeAsync(likeCollections));
        tasks.Add(context.AddRangeAsync(collectionProducts));
        tasks.Add(context.AddRangeAsync(cartItems));
        tasks.Add(context.AddRangeAsync(productImages));
        tasks.Add(context.AddRangeAsync(modelMaterials));
        tasks.Add(context.AddRangeAsync(productSoftwares));
        tasks.Add(context.AddRangeAsync(productTypes));
        tasks.Add(context.AddRangeAsync(transactionDetails));
        tasks.Add(context.AddRangeAsync(blogs));
        tasks.Add(context.AddRangeAsync(blogImages));
        await Task.WhenAll(tasks);
        await context.SaveChangesAsync();
    }
}