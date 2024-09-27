

using Application.Responses.CollectionResponses;
using Domain.Entities;

namespace Application.Mappers.CollectionMappers;

public static class CreateCollectionResponseMapper
{
    public static CreateCollectionResponse MapToCreateCollectionResponse(this Collection collection)
        => new(
            Id: collection.Id,
            CreatedAt: collection.CreatedAt
        );
}