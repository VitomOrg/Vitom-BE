
using Application.Contracts;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Persistence;
public class VitomDBContext(DbContextOptions<VitomDBContext> options) : DbContext(options), IVitomDbContext
{
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<CollectionProduct> CollectionProducts { get; set; }
    public DbSet<CustomColor> CustomColors { get; set; }
    public DbSet<LikeCollection> LikeCollections { get; set; }
    public DbSet<LikeProduct> LikeProducts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductSoftware> ProductSoftwares { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionDetail> TransactionDetails { get; set; }
    public DbSet<Domain.Entities.Type> Types { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserLibrary> UserLibrarys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(e => e.License).HasConversion(v => v.ToString(), v => (LicenseEnum)Enum.Parse(typeof(LicenseEnum), v));
    }
}