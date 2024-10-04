using Application.Mappers.ReviewMappers;
using Application.Responses.RatingResponse.cs;
using Domain.Entities;

namespace Application.Mappers.RatingMappers;

public static class RatingWithReviewMapper
{
    public static RatingWithReviewResponse MapToRatingWithReviewResponse(this IGrouping<int, Review> review)
     => new(
            Key: review.Key,
            ReviewDetails: review.Select(r => r.MapToReviewDetailsResponse())
        );
}