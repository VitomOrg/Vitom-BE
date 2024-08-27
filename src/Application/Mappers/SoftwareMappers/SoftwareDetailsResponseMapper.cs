using Application.Responses.SoftwareResponses;
using Domain.Entities;

namespace Application.Mappers.SoftwareMappers;

public static class SoftwareDetailsResponseMapper
{
    public static SoftwareDetailsResponse MapToSoftwareDetailsResponse(this Software software)
        => new(
            Id: software.Id,
            CreatedAt: software.CreatedAt,
            Name: software.Name,
            Description: software.Description,
            TotalPurchases: software.TotalPurchases
        );
}