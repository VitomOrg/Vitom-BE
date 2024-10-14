using API.Utils;
using Application.Responses.ProductResponses;
using Application.Responses.Shared;
using Application.UC_Product.Queries;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.ProductEndpointHandlers;

public class FetchProductsForEachSoftwareEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender,
        string? type,
        bool ascByCreatedAt = false,
        int pageIndex = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        Result<PaginatedResponse<FetchProductsForEachSoftwareResponse>> result = await sender.Send(new FetchProductsForEachSoftware.Query(
            AscByCreatedAt: ascByCreatedAt,
            Type: type,
            PageIndex: pageIndex,
            PageSize: pageSize
        ), cancellationToken);
        return result.Check();
    }
}