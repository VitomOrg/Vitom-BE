using Application.Responses.LikeProductResponses;
using Domain.Entities;

namespace Application.Mappers.LikeProductMappers;

public static class CreateLikeProductResponseMapper
{
    public static CreateLikeProductResponse MapToCreateLikeProductResponse(this LikeProduct likeProduct)
        => new(likeProduct.Id, likeProduct.CreatedAt, likeProduct.DeletedAt);
}