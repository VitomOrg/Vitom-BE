using Application.Responses.ProductResponses;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mappers.ProductMappers;

public static class ProductDetailsResponseMapper
{
    public static ProductDetailsResponse MapToProductDetailsResponse(this Product product)
        => new(
            Id: product.Id,
            CreatedAt: product.CreatedAt,
            UserId: product.UserId,
            License: Enum.TryParse(product.License.ToString(), out LicenseEnum license) ? ((LicenseEnum)product.License).ToString() : "unknown license",
            Name: product.Name,
            Description: product.Description,
            Types: product.ProductTypes.Where(pt => pt.DeletedAt == null).Select(p => p.Type.Name),
            ImageUrls: product.ProductImages.Where(pi => pi.DeletedAt == null).Select(p => p.Url),
            ModelMaterialUrls: product.ModelMaterials.Where(mm => mm.DeletedAt == null).Select(p => p.Url),
            Price: product.Price,
            DownloadUrl: product.DownloadUrl,
            TotalPurchases: product.TotalPurchases,
            TotalLiked: product.TotalLiked
        );

    public static ProductDetailsResponse MapForProductDetail(this Product product)
        => new(
            Id: product.Id,
            CreatedAt: product.CreatedAt,
            UserId: product.UserId,
            License: Enum.TryParse(product.License.ToString(), out LicenseEnum license) ? ((LicenseEnum)product.License).ToString() : "unknown license",
            Name: product.Name,
            Description: product.Description,
            Types: product.ProductTypes.Where(pt => pt.DeletedAt == null).Select(p => p.Type.Name),
            ImageUrls: product.ProductImages.Where(pi => pi.DeletedAt == null).Select(p => p.Url),
            ModelMaterialUrls: product.ModelMaterials.Where(mm => mm.DeletedAt == null).Select(p => p.Url),
            Price: product.Price,
            DownloadUrl: product.UserLibraries.Any(x => x.DeletedAt == null) == true ? product.DownloadUrl : String.Empty,
            TotalPurchases: product.TotalPurchases,
            TotalLiked: product.TotalLiked
        );
}
