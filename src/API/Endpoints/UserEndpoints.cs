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

        group.MapPut("/artist/{Id}", AssignUserToArtistEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Assign user to artist"))
            .RequireAuthorization();

        group.MapGet("products", FetchLikedProductByUserIdEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get product user liked"))
            .RequireAuthorization();

        group.MapPut("/admin/{Id}", AssignUserToAdminEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Assign user to admin"))
            .RequireAuthorization();
        return group;
    }
}