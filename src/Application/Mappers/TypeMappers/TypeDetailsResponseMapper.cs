using Application.Responses.TypeResponses;
using Type = Domain.Entities.Type;

namespace Application.Mappers.TypeMappers;

public static class TypeDetailsResponseMapper
{
    public static TypeDetailsResponse MapToTypeDetailsResponse(this Type type) =>
        new(Id: type.Id, CreatedAt: type.CreatedAt, Name: type.Name, Description: type.Description);
}
