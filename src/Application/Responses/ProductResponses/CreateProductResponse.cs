namespace Application.Responses.ProductResponses;

public record CreateProductResponse
(
    Guid Id,
    DateTimeOffset CreatedAt
);
