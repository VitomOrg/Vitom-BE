using API.Utils;
using Application.Responses.BlogResponses;
using Application.UC_Blog.Queries;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.BlogEndpointHandler;

public class GetBlogByIdEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender, Guid Id, CancellationToken cancellationToken = default)
    {
        Result<BlogDetailResponse> result = await sender.Send(new GetBlogById.Query(Id), cancellationToken);
        return result.Check();
    }
}