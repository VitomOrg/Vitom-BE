using API.Utils;
using Application.UC_User.Command;
using Ardalis.Result;
using Domain.ExternalEntities;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.UserEndpointHandlers;

public class UpdateUserEndpointHandler
{
    public async static Task<IResult> Handle(ISender sender, Event request, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new UpdateUser.Command(request), cancellationToken);
        return result.Check();
    }
}