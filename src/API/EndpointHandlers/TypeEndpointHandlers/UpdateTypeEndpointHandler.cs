using API.Utils;
using Application.UC_Type.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.TypeEndpointHandlers;

public class UpdateTypeEndpointHandler
{
    public record UpdateTypeRequest(string name, string description);

    public static async Task<IResult> Handle(
        ISender sender,
        Guid id,
        UpdateTypeRequest request,
        CancellationToken cancellationToken = default
    )
    {
        Result result = await sender.Send(
            new UpdateType.Command(Id: id, Name: request.name, Description: request.description),
            cancellationToken
        );

        return result.Check();
    }
}