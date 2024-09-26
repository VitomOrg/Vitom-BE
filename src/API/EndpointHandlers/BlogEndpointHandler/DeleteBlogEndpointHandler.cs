using API.Utils;
using Application.UC_Blog.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.BlogEndpointHandler;

public class DeleteBlogEndpointHandler
{
    public static async Task<IResult> Handle(
        ISender sender,
        Guid Id,
        CancellationToken cancellationToken = default
    )
    {
        Result result = await sender.Send(new DeleteBlog.Command(Id: Id), cancellationToken);
        return result.Check();
    }
}