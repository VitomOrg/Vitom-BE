using Application.Responses.ImageResponses;
using Domain.Entities;

namespace Application.Mappers.ImageMappers;

public static class ImageDetailReponseMapper
{
    public static ImageDetailResponse MapToImageDetailResponse(this ProductImage image)
        => new(
            Id: image.Id,
            Url: image.Url
        );
}