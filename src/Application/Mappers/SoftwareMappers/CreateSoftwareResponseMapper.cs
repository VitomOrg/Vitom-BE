using Application.Responses.SoftwareResponses;
using Domain.Entities;

namespace Application.Mappers.SoftwareMappers;

public static class CreateSoftwareResponseMapper
{
    public static CreateSoftwareResponse MapToCreateSoftwareResponse(this Software software)
        => new(
            Id: software.Id,
            CreatedAt: software.CreatedAt
        );
}