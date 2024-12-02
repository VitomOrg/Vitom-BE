using API.Utils;
using Ardalis.Result;
using MediatR;
using static Application.UC_User.Queries.GetAllUsers;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.UserEndpointHandlers;

public class GetAllUsersEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender, CancellationToken cancellationToken = default)
    {
        Result<List<UserDetailsResponse>> result = await sender.Send(new Query(), cancellationToken);
        return result.Check();
    }
}