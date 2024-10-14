using API.Utils;
using Application.Responses.ProductResponses;
using Application.Responses.Shared;
using Application.UC_User.Queries;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.ProductEndpointHandlers;

public class FetchLikedProductByUserIdEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender,
        bool ascByCreatedAt = false,
        int pageIndex = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        Result<PaginatedResponse<ProductDetailsResponse>> result = await sender.Send(new FetchLikedProductByUserId.Query(
            AscByCreatedAt: ascByCreatedAt,
            PageIndex: pageIndex,
            PageSize: pageSize
        ), cancellationToken);
        return result.Check();
    }
}