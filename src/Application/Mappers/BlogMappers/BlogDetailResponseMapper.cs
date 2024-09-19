using Application.Responses.BlogResponses;
using Domain.Entities;

namespace Application.Mappers.BlogMappers;

public static class BlogDetailResponseMapper
{
    public static BlogDetailResponse MapToBlogDetailResponse(this Blog blog)
        => new(
            Id: blog.Id,
            Title: blog.Title,
            Content: blog.Content,
            UserId: blog.UserId,
            Username: blog.User.Username,
            UserImageUrl: blog.User.ImageUrl,
            CreatedAt: blog.CreatedAt,
            ImageUrl: blog.Images.Select(i => i.Url).ToArray()
            );


}