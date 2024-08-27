using Application.Responses.ReviewResponses;
using Domain.Entities;

namespace Application.Mappers.ReviewMappers;

public static class ReviewDetailsResponseMapper
{
    public static ReviewDetailsResponse MapToReviewDetailsResponse(this Review review)
        => new(
            Id: review.Id,
            CreatedAt: review.CreatedAt,
            ProductId: review.ProductId,
            ProductName: review.Product?.Name ?? string.Empty,
            UserId: review.UserId,
            Username: review.User?.Username ?? string.Empty,
            Rating: review.Rating,
            Content: review.Content
        );
}