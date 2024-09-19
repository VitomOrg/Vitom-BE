using API.Utils;
using Application.Responses.BlogResponses;
using Application.UC_Blog;
using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.BlogEndpointHandler;

public class CreateBlogEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender,
        [FromForm] string Title,
        [FromForm] string Content,
        IFormFileCollection images,
        CancellationToken cancellationToken = default)
    {
        Result<CreateBlogResponses> result = await sender.Send(new CreateBlog.Command(
            Title: Title,
            Content: Content,
            Images: images
        ), cancellationToken);
        return result.Check();
    }
}