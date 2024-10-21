using API.Utils;
using Application.Responses.BlogResponses;
using Application.UC_Blog.Commands;
using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.BlogEndpointHandler;

public class CreateBlogEndpointHandler
{
    public record CreateBlogRequests(
        string title,
        string content,
        List<Stream> images
    );
    public static async Task<IResult> Handle(ISender sender,
        CreateBlogRequests request,
        CancellationToken cancellationToken = default)
    {
        Result<CreateBlogResponses> result = await sender.Send(new CreateBlog.Command(
            Title: request.title,
            Content: request.content,
            Images: request.images
        ), cancellationToken);
        return result.Check();
    }
}