using Application.Responses.BlogResponses;
using Domain.Entities;

namespace Application.Mappers.BlogMappers;

public static class CreateBlogResponseMapper
{
    public static CreateBlogResponses MapToCreateBlogResponse(this Blog blog)
        => new(
            Id: blog.Id,
            Title: blog.Title,
            Content: blog.Content,
            CreatedAt: blog.CreatedAt,
            ImageUrl: blog.Images.Select(i => i.Url).ToArray()
        );
}