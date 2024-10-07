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
        int pageSize = 10,
        int pageIndex = 1,
        CancellationToken cancellationToken = default
    )
    {
        Result<PaginatedResponse<AllCollectionDetailsResponse>> result = await sender.Send(
            new ViewLikedCollections.Query(pageSize, pageIndex),
            cancellationToken
        );

        return result.Check();
    }
}
