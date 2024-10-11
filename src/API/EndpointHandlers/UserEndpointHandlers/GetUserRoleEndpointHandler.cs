using API.Utils;
using Application.UC_User.Queries;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.UserEndpointHandlers;

public class GetUserRoleEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender, CancellationToken cancellationToken = default)
    {
        Result<string> result = await sender.Send(new GetUserRole.Command(), cancellationToken);
        return result.Check();
    }
}