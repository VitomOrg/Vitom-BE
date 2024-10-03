using API.Utils;
using Application.UC_User.Command;
using Ardalis.Result;
using Domain.ExternalEntities;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.UserEndpointHandlers;

public class CreateUserEndpointHandler
{

    public static async Task<IResult> Handle(ISender sender, ClerkUser request, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new CreateUser.Command(request), cancellationToken);
        return result.Check();
    }
}