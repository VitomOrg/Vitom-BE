using API.Utils;
using Application.Responses.ProductResponses;
using Application.Responses.Shared;
using Application.UC_User.Queries;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.UserEndpointHandlers;

public class FetchLikedProductByUserIdEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender,
        bool AscByCreatedAt = false,
        int PageIndex = 1,
        int PageSize = 10,
        CancellationToken cancellationToken = default)
    {
        Result<PaginatedResponse<ProductDetailsResponse>> result = await sender.Send(new FetchLikedProductByUserId.Query(
            AscByCreatedAt: AscByCreatedAt,
            PageIndex: PageIndex,
            PageSize: PageSize
        ), cancellationToken);
        return result.Check();
    }
}
