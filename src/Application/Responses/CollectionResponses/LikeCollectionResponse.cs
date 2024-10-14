namespace Application.Responses.CollectionResponses;

public record LikeCollectionResponse(Guid Id, DateTimeOffset CreatedAt, DateTimeOffset? DeletedAt);