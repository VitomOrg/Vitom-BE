using API.Utils;
using Application.Responses.CollectionResponses;
using Application.Responses.Shared;
using Application.UC_Collection.Queries;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.CollectionEndpointHandlers;

public class ViewLikedCollectionEndpointHandler
{
    public static async Task<IResult> Handle(
        ISender sender,
        int PageSize = 10,
        int PageIndex = 1,
        CancellationToken cancellationToken = default
    )
    {
        Result<PaginatedResponse<AllCollectionDetailsResponse>> result = await sender.Send(
            new ViewLikedCollections.Query(PageSize, PageIndex),
            cancellationToken
        );

        return result.Check();
    }
}
