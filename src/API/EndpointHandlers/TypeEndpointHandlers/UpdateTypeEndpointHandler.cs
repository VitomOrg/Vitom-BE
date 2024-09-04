using API.Utils;
using Application.UC_Type.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.TypeEndpointHandlers;

public class UpdateTypeEndpointHandler
{
    public record UpdateTypeRequest(string Name, string Description);

    public static async Task<IResult> Handle(
        ISender sender,
        Guid Id,
        UpdateTypeRequest request,
        CancellationToken cancellationToken = default
    )
    {
        Result result = await sender.Send(
            new UpdateType.Command(Id: Id, Name: request.Name, Description: request.Description),
            cancellationToken
        );

        return result.Check();
    }
}
