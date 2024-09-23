using Application.Responses.ReviewResponses;
using Domain.Entities;

namespace Application.Mappers.ReviewMappers;

public static class CreateReviewResponseMapper
{
    public static CreateReviewResponse MapToCreateReviewResponse(this Review review)
        => new(
            Id: review.Id,
            CreateAt: review.CreatedAt
        );
}