using Application.Responses.MaterialResponses;
using Domain.Entities;

namespace Application.Mappers.MaterialMappers;

public static class MaterialDetailResponseMapper
{
    public static MaterialDetailResponse MapToMaterialDetailResponse(this ModelMaterial material)
        => new(
            Id: material.Id,
            Url: material.Url
        );
}