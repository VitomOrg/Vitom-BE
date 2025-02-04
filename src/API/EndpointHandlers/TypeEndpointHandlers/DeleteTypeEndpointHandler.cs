using API.Utils;
using Application.UC_Type.Commands;
using Ardalis.Result;
using MediatR;

namespace API.EndpointHandlers.TypeEndpointHandlers;

public class DeleteTypeEndpointHandler
{
    public static async Task<Microsoft.AspNetCore.Http.IResult> Handle(
        ISender sender,
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        Result result = await sender.Send(new DeleteType.Command(Id: id), cancellationToken);

        return result.Check();
    }
}