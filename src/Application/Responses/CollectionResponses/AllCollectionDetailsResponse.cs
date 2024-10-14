using Domain.Entities;

namespace Application.Responses.CollectionResponses;

public record AllCollectionDetailsResponse(
    Guid Id,
    string Name,
    string Description,
    int TotalLiked,
    SimpleUserResponse User
);

public record SimpleUserResponse(string Id, string Username, string Avatar);