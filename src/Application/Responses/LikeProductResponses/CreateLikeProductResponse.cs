namespace Application.Responses.LikeProductResponses;

public record CreateLikeProductResponse
(
    Guid Id,
    DateTimeOffset CreatedAt,
    DateTimeOffset? DeleteAt
);