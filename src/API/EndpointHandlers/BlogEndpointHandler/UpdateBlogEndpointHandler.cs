using API.Utils;
using Application.Responses.BlogResponses;
using Application.UC_Blog.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.BlogEndpointHandler;

public class UpdateBlogEndpointHandler
{
    public record UpdateBlogRequest(
        Guid id,
        string title,
        string content,
        List<Stream> images
    );
    public static async Task<IResult> Handle(
        ISender sender,
        UpdateBlogRequest request,
      CancellationToken cancellationToken = default
    )
    {
        Result<UpdateBlogResponse> result = await sender.Send(new UpdateBlog.Command(
            Id: request.id,
            Title: request.title,
            Content: request.content,
            Images: request.images
        ), cancellationToken);
        return result.Check();
    }
}