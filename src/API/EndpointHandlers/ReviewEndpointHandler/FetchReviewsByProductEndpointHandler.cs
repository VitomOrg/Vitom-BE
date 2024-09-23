using API.Utils;
using Application.Responses.ReviewResponses;
using Application.Responses.Shared;
using Application.UC_Review.Queries;
using Ardalis.Result;
using MediatR;

namespace API.EndpointHandlers.ReviewEndpointHandler;

public class FetchReviewsByProductEndpointHandler
{
    public static async Task<Microsoft.AspNetCore.Http.IResult> Handle(ISender sender, Guid productId, int pageSize = 10, int pageIndex = 1, CancellationToken cancellationToken = default)
    {
        Result<PaginatedResponse<ReviewDetailsResponse>> result = await sender.Send(new FetchReviewsOfProduct.Query(
            ProductId: productId,
            PageSize: pageSize,
            PageIndex: pageIndex
        ), cancellationToken);
        return result.Check();
    }
}