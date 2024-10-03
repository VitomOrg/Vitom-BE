using Application.Contracts;
using Domain.Entities;
using Domain.Entities.Report;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class VitomDBContext(DbContextOptions<VitomDBContext> options)
    : DbContext(options),
        IVitomDbContext
{
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<CollectionProduct> CollectionProducts { get; set; }
    public DbSet<LikeCollection> LikeCollections { get; set; }
    public DbSet<LikeProduct> LikeProducts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ModelMaterial> ModelMaterials { get; set; }
    public DbSet<ProductSoftware> ProductSoftwares { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionDetail> TransactionDetails { get; set; }
    public DbSet<Domain.Entities.Type> Types { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserLibrary> UserLibrarys { get; set; }
    public DbSet<MonthlyIncome> MonthlyIncomes { get; set; }
    public DbSet<SystemTotal> SystemTotals { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<BlogImage> BlogImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Defining composite key
        modelBuilder.Entity<MonthlyIncome>().HasKey(mi => new { mi.Year, mi.Month });
        modelBuilder.Entity<Blog>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<BlogImage>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<Cart>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<CartItem>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<Collection>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<CollectionProduct>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<LikeCollection>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<LikeProduct>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<Product>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<ProductImage>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<ProductSoftware>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<ProductType>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<Review>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<Software>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<Transaction>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<TransactionDetail>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<Domain.Entities.Type>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<User>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<UserLibrary>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<ModelMaterial>().HasQueryFilter(t => t.DeletedAt == null);
        base.OnModelCreating(modelBuilder);
    }
}
