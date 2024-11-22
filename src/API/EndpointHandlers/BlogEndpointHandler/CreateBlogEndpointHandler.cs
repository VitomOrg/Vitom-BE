using API.Utils;
using Application.Responses.BlogResponses;
using Application.UC_Blog.Commands;
using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.BlogEndpointHandler;

public class CreateBlogEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender,
        CreateBlogCommand command,
        CancellationToken cancellationToken = default)
    {
        Result<CreateBlogResponses> result = await sender.Send(new CreateBlog.Command(
            Title: command.title,
            Content: command.content,
            Images: command.images
        ), cancellationToken);
        return result.Check();
    }

    public record CreateBlogCommand(
        string title,
        string content,
        string[] images
    );
}