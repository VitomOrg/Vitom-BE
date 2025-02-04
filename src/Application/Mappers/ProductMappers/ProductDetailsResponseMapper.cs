using Application.Mappers.ImageMappers;
using Application.Mappers.MaterialMappers;
using Application.Responses.ProductResponses;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;

namespace Application.Mappers.ProductMappers;

public static class ProductDetailsResponseMapper
{
    public static ProductDetailsResponse MapToProductDetailsResponse(this Product product)
        => new(
            Id: product.Id,
            CreatedAt: product.CreatedAt,
            UserId: product.UserId,
            License: Enum.TryParse(product.License.ToString(), out LicenseEnum license) ? product.License.ToString() : "unknown license",
            Name: product.Name,
            Description: product.Description,
            Types: product.ProductTypes.Where(pt => pt.DeletedAt == null).Select(pt => new TypesResponse(pt.Type.Id, pt.Type.Name)).ToArray(),
            Softwares: product.ProductSoftwares.Where(p => p.DeletedAt == null).Select(p => new SoftwaresResponse(p.Software.Id, p.Software.Name)).ToArray(),
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

    public static ProductDetailsResponse MapForProductDetail(this Product product, CurrentUser currentUser)
        => new(
            Id: product.Id,
            CreatedAt: product.CreatedAt,
            UserId: product.UserId,
            License: Enum.TryParse(product.License.ToString(), out LicenseEnum license) ? product.License.ToString() : "unknown license",
            Name: product.Name,
            Description: product.Description,
            Types: product.ProductTypes.Where(pt => pt.DeletedAt == null).Select(pt => new TypesResponse(pt.Type.Id, pt.Type.Name)).ToArray(),
            Softwares: product.ProductSoftwares.Where(p => p.DeletedAt == null).Select(p => new SoftwaresResponse(p.Software.Id, p.Software.Name)).ToArray(),
            Images: product.ProductImages.Where(pi => pi.DeletedAt == null).Select(p => p.MapToImageDetailResponse()),
            ModelMaterials: product.ModelMaterials.Where(mm => mm.DeletedAt == null).Select(p => p.MapToMaterialDetailResponse()),
            FbxUrl: product.Model?.Fbx,
            ObjUrl: product.Model?.Obj,
            GlbUrl: product.Model?.Glb,
            Price: product.Price,
            DownloadUrl: (product.UserLibraries.Any(x => x.DeletedAt == null && x.UserId == currentUser.User?.Id)
                            || product.License.Equals(LicenseEnum.Free)) ? product.DownloadUrl : string.Empty,
            TotalPurchases: product.TotalPurchases,
            TotalLiked: product.TotalLiked,
            IsLiked: product.LikeProducts.Any(x => x.UserId == currentUser.User?.Id)
        );
}