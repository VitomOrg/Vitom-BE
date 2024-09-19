using API.Utils;
using Application.Responses.BlogResponses;
using Application.Responses.Shared;
using Application.UC_Blog.Queries;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.BlogEndpointHandler;

public class GetBlogsEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender, int PageSize = 10, int PageIndex = 1, string Keyword = "", CancellationToken cancellationToken = default)
    {
        Result<PaginatedResponse<BlogDetailResponse>> result = await sender.Send(new GetBlogs.Query(
            PageSize: PageSize,
            PageIndex: PageIndex,
            Keyword: Keyword
        ), cancellationToken);
        return result.Check();
    }
}