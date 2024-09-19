namespace Application.Responses.BlogResponses;

public record BlogDetailResponse(
    Guid Id,
    string Title,
    string Content,
    string UserId,
    string Username,
    string UserImageUrl,
    DateTimeOffset CreatedAt,
    string[] ImageUrl
);