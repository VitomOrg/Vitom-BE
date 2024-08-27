namespace Application.Responses.ReviewResponses;

public record ReviewDetailsResponse(
    Guid Id,
    DateTimeOffset CreatedAt,
    Guid ProductId,
    string ProductName,
    string UserId,
    string Username,
    int Rating,
    string Content
);