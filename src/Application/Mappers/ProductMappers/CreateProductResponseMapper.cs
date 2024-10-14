using Application.Responses.ProductResponses;
using Domain.Entities;

namespace Application.Mappers.ProductMappers;

public static class CreateProductResponseMapper
{
    public static CreateProductResponse MapToCreateProductResponse(this Product product)
        => new(
            Id: product.Id,
            CreatedAt: product.CreatedAt
        );
}