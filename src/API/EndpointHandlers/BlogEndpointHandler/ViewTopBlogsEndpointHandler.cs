using API.Utils;
using Application.UC_Blog.Queries;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.BlogEndpointHandler;

public class ViewTopBlogsEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender, CancellationToken cancellationToken = default)
    {
        Result<ViewTopBlogs.Response> result = await sender.Send(new ViewTopBlogs.Query(), cancellationToken);
        return result.Check();
    }
}