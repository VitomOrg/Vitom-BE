namespace Application.Responses.CollectionResponses;

public record CreateCollectionResponse(
    Guid Id,
    DateTimeOffset CreatedAt
);