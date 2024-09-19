namespace Application.Responses.BlogResponses;

public record CreateBlogResponses(
    Guid Id,
    string Title,
    string Content,
    DateTimeOffset CreatedAt,
    string[] ImageUrl
);