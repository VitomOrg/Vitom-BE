using API.Utils;
using Application.Responses.TypeResponses;
using Application.UC_Type.Commands;
using Ardalis.Result;
using MediatR;

namespace API.EndpointHandlers.TypeEndpointHandlers;

public class CreateTypeEndpointHandler
{
    public record CreateTypeRequest(string name, string description);

    public static async Task<Microsoft.AspNetCore.Http.IResult> Handle(
        ISender sender,
        CreateTypeRequest request,
        CancellationToken cancellationToken = default
    )
    {
        Result<CreateTypeResponse> result = await sender.Send(
            new CreateType.Command(Name: request.name, Description: request.description),
            cancellationToken
        );

        return result.Check();
    }
}