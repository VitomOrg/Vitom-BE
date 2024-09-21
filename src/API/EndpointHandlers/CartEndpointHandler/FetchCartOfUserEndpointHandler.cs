using API.Utils;
using Application.Responses.CartResponses;
using Application.Responses.ProductResponses;
using Application.Responses.Shared;
using Application.UC_Cart.Queries;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.CartEndpointHandler;

public class FetchCartOfUserEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender,
        bool AscByCreatedAt = false,
        int pageIndex = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        Result<PaginatedResponse<CartItemResponse>> result = await sender.Send(new FetchCartOfUser.Query(
            AscByCreatedAt: AscByCreatedAt,
            PageIndex: pageIndex,
            PageSize: pageSize
        ), cancellationToken);

        return result.Check();
    }
}