using Application.Responses.CollectionResponses;
using Domain.Entities;

namespace Application.Mappers.CollectionMappers;

public static class AllCollectionDetailsResponseMapper
{
    public static AllCollectionDetailsResponse MapToAllCollectionDetailsResponse(
        this Collection collection
    ) =>
        new(
            Id: collection.Id,
            Name: collection.Name,
            Description: collection.Description,
            TotalLiked: collection.TotalLiked,
            User: collection.User.MapToSimpleUserResponse()
        );

    public static SimpleUserResponse MapToSimpleUserResponse(this User user) =>
        new(Id: user.Id, Username: user.Username, Avatar: user.ImageUrl);
}