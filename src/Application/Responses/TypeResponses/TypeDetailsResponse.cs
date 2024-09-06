namespace Application.Responses.TypeResponses;

public record TypeDetailsResponse(
    Guid Id,
    DateTimeOffset CreatedAt,
    string Name,
    string Description
);
