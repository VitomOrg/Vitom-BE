
using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Primitives;

namespace Domain.Entities;

public class Product : Entity
{
    public required string UserId { get; set; }
    public LicenseEnum License { get; set; } = LicenseEnum.Free;

    [MaxLength(100)]
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;

    [Range(0, 9999999999)]
    [RegularExpression(@"^\d+(\.\d{1,2})?$")]
    public decimal Price { get; set; } = 0;
    public required string DownloadUrl { get; set; }

    [Range(0, int.MaxValue)]
    public int TotalPurchases { get; set; } = 0;

    [Range(0, int.MaxValue)]
    public int TotalLiked { get; set; } = 0;
    public User User { get; set; } = null!;
    public Model Model { get; set; } = null!;
    public ICollection<UserLibrary> UserLibraries { get; set; } = [];
    public ICollection<LikeProduct> LikeProducts { get; set; } = [];
    public ICollection<CollectionProduct> CollectionProducts { get; set; } = [];
    public ICollection<CartItem> CartItems { get; set; } = [];
    public ICollection<ProductType> ProductTypes { get; set; } = [];
    public ICollection<ProductSoftware> ProductSoftwares { get; set; } = [];
    public ICollection<ProductImage> ProductImages { get; set; } = [];
    public ICollection<ModelMaterial> ModelMaterials { get; set; } = [];
    public ICollection<TransactionDetail> TransactionDetails { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];

    public void Update(LicenseEnum license, string name, string description, decimal price, string downloadUrl)
    {
        License = license;
        Name = name;
        Description = description;
        Price = price;
        DownloadUrl = downloadUrl;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
