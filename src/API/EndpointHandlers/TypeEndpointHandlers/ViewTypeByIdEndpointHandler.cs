using API.Utils;
using Application.Responses.TypeResponses;
using Application.UC_Type.Queries;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.TypeEndpointHandlers;

public class ViewTypeByIdEndpointHandler
{
    public static async Task<IResult> Handle(
        ISender sender,
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        Result<TypeDetailsResponse> result = await sender.Send(new ViewTypeById.Query(id), cancellationToken);
        return result.Check();
    }
}