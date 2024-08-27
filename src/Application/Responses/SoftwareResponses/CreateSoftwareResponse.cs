namespace Application.Responses.SoftwareResponses;

public record CreateSoftwareResponse(
    Guid Id,
    DateTimeOffset CreatedAt
);