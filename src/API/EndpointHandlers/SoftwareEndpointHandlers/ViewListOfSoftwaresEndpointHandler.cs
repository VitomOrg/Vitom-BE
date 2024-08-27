using API.Utils;
using Application.Responses.Shared;
using Application.Responses.SoftwareResponses;
using Application.UC_Software.Queries;
using Ardalis.Result;
using MediatR;

namespace API.EndpointHandlers.SoftwareEndpointHandlers;

public class ViewListOfSoftwaresEndpointHandler
{
    public static async Task<Microsoft.AspNetCore.Http.IResult> Handle(ISender sender, string keyword = "", int pageSize = 10, int pageIndex = 1, CancellationToken cancellationToken = default)
    {
        Result<PaginatedResponse<SoftwareDetailsResponse>> result = await sender.Send(
            new ViewListOfSoftware.Query(
                Keyword: keyword,
                PageSize: pageSize,
                PageIndex: pageIndex
            )
            , cancellationToken);
        return result.Check();
    }
}