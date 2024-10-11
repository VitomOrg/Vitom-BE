using API.EndpointHandlers.UserEndpointHandlers;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoint(this RouteGroupBuilder group)
    {
        // GET
        group
            .MapGet("", GetUserRoleEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get current users Role"))
            .RequireAuthorization();
        // PUT
        group.MapPut("/artist", AssignUserToArtistEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Assign user to artist"))
            .RequireAuthorization();
        // PUT
        group.MapPut("/admin", AssignUserToAdminEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Assign user to admin"))
            .RequireAuthorization();
        // WEBHOOK
        group.MapPost("/created", CreateUserEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Create new user for WEBHOOK"));
        group.MapPost("/updated", CreateUserEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Update new user for WEBHOOK"));
        return group;
    }
}