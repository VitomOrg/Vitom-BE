using API.EndpointHandlers.UserEndpointHandler;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetUserByIdEndpointHandler.Handle).WithMetadata(new SwaggerOperationAttribute("con cac"));
        return group;
    }
}