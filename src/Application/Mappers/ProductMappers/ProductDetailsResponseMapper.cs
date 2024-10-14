using Application.Mappers.ImageMappers;
using Application.Mappers.MaterialMappers;
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
            Softwares: product.ProductSoftwares.Where(pt => pt.DeletedAt == null).Select(p => p.Software.Name),
            Images: product.ProductImages.Where(pi => pi.DeletedAt == null).Select(p => p.MapToImageDetailResponse()),
            ModelMaterials: product.ModelMaterials.Where(mm => mm.DeletedAt == null).Select(p => p.MapToMaterialDetailResponse()),
            FbxUrl: product.Model?.Fbx,
            ObjUrl: product.Model?.Obj,
            GlbUrl: product.Model?.Glb,
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
            Softwares: product.ProductSoftwares.Where(pt => pt.DeletedAt == null).Select(p => p.Software.Name),
            Images: product.ProductImages.Where(pi => pi.DeletedAt == null).Select(p => p.MapToImageDetailResponse()),
            ModelMaterials: product.ModelMaterials.Where(mm => mm.DeletedAt == null).Select(p => p.MapToMaterialDetailResponse()),
            FbxUrl: product.Model?.Fbx,
            ObjUrl: product.Model?.Obj,
            GlbUrl: product.Model?.Glb,
            Price: product.Price,
            DownloadUrl: product.UserLibraries.Any(x => x.DeletedAt == null) == true ? product.DownloadUrl : String.Empty,
            TotalPurchases: product.TotalPurchases,
            TotalLiked: product.TotalLiked
        );
}