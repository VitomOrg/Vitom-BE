using API.Utils;
using Application.Responses.ProductResponses;
using Application.Responses.Shared;
using Application.UC_Product.Queries;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.ProductEndpointHandlers;

public class FetchDownloadedProductsEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender,
        bool ascByCreatedAt = false,
        int pageSize = 10,
        int pageIndex = 1,
        CancellationToken cancellationToken = default)
    {
        Result<PaginatedResponse<ProductDetailsResponse>> result = await sender.Send(new FetchDownloadedProducts.Query(
            AscByCreatedAt: ascByCreatedAt,
            PageSize: pageSize,
            PageIndex: pageIndex
        ), cancellationToken);
        return result.Check();
    }
}