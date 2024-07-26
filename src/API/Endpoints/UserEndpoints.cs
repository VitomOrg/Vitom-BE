using API.EndpointHandlers.UserEndpointHandlers;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("", GetAllUsersEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get all users"));
        return group;
    }
}