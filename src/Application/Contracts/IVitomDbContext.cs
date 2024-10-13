using Domain.Entities;
using Domain.Entities.Report;
using Microsoft.EntityFrameworkCore;
using Type = Domain.Entities.Type;

namespace Application.Contracts;

public interface IVitomDbContext
{
    DbSet<Cart> Carts { get; set; }
    DbSet<CartItem> CartItems { get; set; }
    DbSet<Collection> Collections { get; set; }
    DbSet<CollectionProduct> CollectionProducts { get; set; }
    DbSet<LikeCollection> LikeCollections { get; set; }
    DbSet<LikeProduct> LikeProducts { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<ProductImage> ProductImages { get; set; }
    DbSet<ModelMaterial> ModelMaterials { get; set; }
    DbSet<ProductSoftware> ProductSoftwares { get; set; }
    DbSet<ProductType> ProductTypes { get; set; }
    DbSet<Review> Reviews { get; set; }
    DbSet<Software> Softwares { get; set; }
    DbSet<Transaction> Transactions { get; set; }
    DbSet<TransactionDetail> TransactionDetails { get; set; }
    DbSet<Type> Types { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<UserLibrary> UserLibrarys { get; set; }
    DbSet<MonthlyIncome> MonthlyIncomes { get; set; }
    DbSet<SystemTotal> SystemTotals { get; set; }
    DbSet<Blog> Blogs { get; set; }
    DbSet<BlogImage> BlogImages { get; set; }
    DbSet<Model> Models { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);
}
