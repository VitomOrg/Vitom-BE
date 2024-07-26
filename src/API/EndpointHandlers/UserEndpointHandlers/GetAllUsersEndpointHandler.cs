using API.Utils;
using Application.UC_User;
using Ardalis.Result;
using Domain.Entities;
using MediatR;

namespace API.EndpointHandlers.UserEndpointHandlers;

public class GetAllUsersEndpointHandler
{
    public static async Task<Microsoft.AspNetCore.Http.IResult> Handle(ISender sender, int PageNumber = 1, int PageSize = 10, CancellationToken cancellationToken = default!)
    {
        Result<IEnumerable<User>> result = await sender.Send(new GetAllUsers.GetAllUsersQuery(PageNumber: PageNumber,
                                                                                            PageSize: PageSize), cancellationToken);
        return result.Check();
    }
}