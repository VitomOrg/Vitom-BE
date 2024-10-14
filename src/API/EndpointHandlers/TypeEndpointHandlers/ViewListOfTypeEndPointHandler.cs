using API.Utils;
using Application.Responses.Shared;
using Application.Responses.TypeResponses;
using Application.UC_Type.Queries;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.TypeEndpointHandlers;

public class ViewListOfTypeEndpointHandler
{
    public static async Task<IResult> Handle(
        ISender sender,
        string keyword = "",
        int pageSize = 10,
        int pageIndex = 1,
        CancellationToken cancellationToken = default
    )
    {
        Result<PaginatedResponse<TypeDetailsResponse>> result = await sender.Send(
            new ViewListOfType.Query(Keyword: keyword, PageSize: pageSize, PageIndex: pageIndex),
            cancellationToken
        );

        return result.Check();
    }
}