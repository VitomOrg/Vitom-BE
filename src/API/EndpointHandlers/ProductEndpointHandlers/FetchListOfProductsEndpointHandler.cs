using API.Utils;
using Application.Responses.ProductResponses;
using Application.Responses.Shared;
using Application.UC_Product.Queries;
using Ardalis.Result;
using Domain.Enums;
using MediatR;

namespace API.EndpointHandlers.ProductEndpointHandlers;

public class FetchListOfProductsEndpointHandler
{
    public static async Task<Microsoft.AspNetCore.Http.IResult> Handle(ISender sender,
        LicenseEnum? license,
        string? search,
        string? type,
        decimal priceFrom = 0,
        decimal priceTo = int.MaxValue,
        bool ascByCreatedAt = false,
        int pageSize = 10,
        int pageIndex = 1,
        CancellationToken cancellationToken = default)
    {
        Result<PaginatedResponse<ProductDetailsResponse>> result = await sender.Send(new FetchListOfProducts.Query(
            Search: search,
            Type: type,
            License: license,
            PriceFrom: priceFrom,
            PriceTo: priceTo,
            AscByCreatedAt: ascByCreatedAt,
            PageSize: pageSize,
            PageIndex: pageIndex
        ), cancellationToken);
        return result.Check();
    }
}
