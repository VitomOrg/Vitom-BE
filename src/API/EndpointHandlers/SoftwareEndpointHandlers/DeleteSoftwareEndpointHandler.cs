using API.Utils;
using Application.UC_Software.Commands;
using Ardalis.Result;
using MediatR;

namespace API.EndpointHandlers.SoftwareEndpointHandlers;

public class DeleteSoftwareEndpointHandler
{
    public static async Task<Microsoft.AspNetCore.Http.IResult> Handle(ISender sender, Guid Id, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new DeleteSoftware.Command(Id: Id), cancellationToken);
        return result.Check();
    }
}