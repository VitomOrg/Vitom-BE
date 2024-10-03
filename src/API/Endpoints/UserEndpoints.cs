using API.EndpointHandlers.UserEndpointHandlers;
using Domain.Entities;
using Domain.Primitives;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoint(this RouteGroupBuilder group)
    {
        // testing
        group.MapGet("", (CurrentUser currentUser) =>
        {
            if (currentUser.User is not null)
            {
                User resultingUser = currentUser.User;
                return $"{resultingUser.Id} - {resultingUser.Username}";
            }
            return "Hello World";
        }).RequireAuthorization();

        group.MapPut("/artist", AssignUserToArtistEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Assign user to artist"))
            .RequireAuthorization();

        group.MapPut("/admin", AssignUserToAdminEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Assign user to admin"))
            .RequireAuthorization();

        group.MapPost("/created", CreateUserEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Create new user"));
        return group;
    }
}