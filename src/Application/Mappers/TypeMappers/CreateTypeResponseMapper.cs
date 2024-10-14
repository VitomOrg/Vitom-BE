using Application.Responses.TypeResponses;
using Type = Domain.Entities.Type;

namespace Application.Mappers.TypeMappers;

public static class CreateTypeResponseMapper
{
    public static CreateTypeResponse MapToCreateTypeResponse(this Type type) =>
        new(Id: type.Id, CreatedAt: type.CreatedAt);
}