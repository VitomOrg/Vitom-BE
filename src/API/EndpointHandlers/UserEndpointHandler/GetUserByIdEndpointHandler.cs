using API.Utils;
using Application.UC_User;
using MediatR;

namespace API.EndpointHandlers.UserEndpointHandler;

public class GetUserByIdEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender, Guid id, CancellationToken cancellationToken = default)
        => (await sender.Send(new GetUserByIdQuery(Id: id), cancellationToken)).Check();
}