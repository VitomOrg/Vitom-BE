using API.Utils;
using Application.UC_User.Command;
using Ardalis.Result;
using MediatR;

namespace API.EndpointHandlers.UserEndpointHandlers;

public class AssignUserToArtistEndpointHandler
{
    public static async Task<Microsoft.AspNetCore.Http.IResult> Handle(ISender sender, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new AssignUserToArtist.Command(), cancellationToken);
        return result.Check();
    }
}