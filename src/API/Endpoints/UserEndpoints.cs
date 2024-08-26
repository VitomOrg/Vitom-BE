using Domain.Entities;
using Domain.Primitives;

namespace API.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("", (CurrentUser currentUser) =>
        {
            if (currentUser.User is not null)
            {
                User resultingUser = currentUser.User;
                return $"{resultingUser.Id} - {resultingUser.Username}";
            }
            return "Hello World";
        });
        return group;
    }
}