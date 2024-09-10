using Application.Mappers.ProductMappers;
using Application.Responses.ProductResponses;
using Domain.Entities;

namespace Application.Mappers.SoftwareMappers;

public static class FetchProductsForEachSoftwareResponseMapper
{
    public static FetchProductsForEachSoftwareResponse MapToFetchProductsForEachSoftwareResponse(this Software software, bool AscByCreateAt)
        => new(
            Software: software.Name,
            Products: (AscByCreateAt
            ? software.ProductSoftwares.Select(ps => ps.Product)
                .OrderBy(p => p.CreatedAt)  // Ascending if AscByCreateAt is true
            : software.ProductSoftwares.Select(ps => ps.Product)
                .OrderByDescending(p => p.CreatedAt))  // Descending if AscByCreateAt is false
            .Select(ProductDetailsResponseMapper.MapToProductDetailsResponse)
            .Take(8)
        );

}