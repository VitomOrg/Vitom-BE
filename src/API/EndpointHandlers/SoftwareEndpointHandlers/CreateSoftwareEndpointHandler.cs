using API.Utils;
using Application.Responses.SoftwareResponses;
using Application.UC_Software.Commands;
using Ardalis.Result;
using MediatR;

namespace API.EndpointHandlers.SoftwareEndpointHandlers;

public class CreateSoftwareEndpointHandler
{
    public static async Task<Microsoft.AspNetCore.Http.IResult> Handle(ISender sender, CreateSoftwareRequest request, CancellationToken cancellationToken = default)
    {
        Result<CreateSoftwareResponse> result = await sender.Send(new CreateSoftware.Command(
            Name: request.Name,
            Description: request.Description
        ), cancellationToken);
        return result.Check();
    }
    public record CreateSoftwareRequest(string Name, string Description);
}