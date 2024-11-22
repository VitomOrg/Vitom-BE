using API.Utils;
using Application.Responses.BlogResponses;
using Application.UC_Blog.Commands;
using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.BlogEndpointHandler;

public class UpdateBlogEndpointHandler
{
    public static async Task<IResult> Handle(
        ISender sender,
        Guid id,
        UpdateBlogCommand command,
      CancellationToken cancellationToken = default
    )
    {
        Result<UpdateBlogResponse> result = await sender.Send(new UpdateBlog.Command(
            Id: id,
            Title: command.title,
            Content: command.content,
            Images: command.images
        ), cancellationToken);
        return result.Check();
    }

    public record UpdateBlogCommand(
        string title,
        string content,
        string[] images
    );
}