using API.Utils;
using Application.Responses.CollectionResponses;
using Application.Responses.Shared;
using Application.UC_Collection.Queries;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.CollectionEndpointHandlers;

public class ViewAllPublicCollectionEndpointHandler
{
    public static async Task<IResult> Handle(
        ISender sender,
        int pageSize = 10,
        int pageIndex = 1,
        CancellationToken cancellationToken = default
    )
    {
        Result<PaginatedResponse<AllCollectionDetailsResponse>> result = await sender.Send(
            new ViewAllPublicCollections.Query(PageSize: pageSize, PageIndex: pageIndex),
            cancellationToken
        );

        return result.Check();
    }
}
