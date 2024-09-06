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
            Price: product.Price,
            DownloadUrl: product.DownloadUrl,
            TotalPurchases: product.TotalPurchases,
            TotalLiked: product.TotalLiked
        );
}
