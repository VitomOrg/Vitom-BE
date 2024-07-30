using API.Utils;
using Application.UC_User;
using MediatR;

namespace API.EndpointHandlers.UserEndpointHandlers;

public class RemoveUserEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender, Guid UserId) => (await sender.Send(new RemoveUser.RemoveUserQuery(UserId))).Check();
}