using API.Utils;
using Application.UC_Software.Commands;
using Ardalis.Result;
using MediatR;

namespace API.EndpointHandlers.SoftwareEndpointHandlers;

public class UpdateSoftwareEndpointHandler
{
    public static async Task<Microsoft.AspNetCore.Http.IResult> Handle(ISender sender, Guid id, UpdateSoftwareRequest request, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new UpdateSoftware.Command(
            Id: id,
            Name: request.Name,
            Description: request.Description
        ), cancellationToken);
        return result.Check();
    }
    public record UpdateSoftwareRequest(
        string Name,
        string Description
    );
}