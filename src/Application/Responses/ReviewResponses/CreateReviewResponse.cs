namespace Application.Responses.ReviewResponses;

public record CreateReviewResponse
(
    Guid Id,
    DateTimeOffset CreateAt
);