namespace Application.Responses.SoftwareResponses;

public record SoftwareDetailsResponse(
    Guid Id,
    DateTimeOffset CreatedAt,
    string Name,
    string Description,
    int TotalPurchases
);