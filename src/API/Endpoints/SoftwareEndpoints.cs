using API.EndpointHandlers.SoftwareEndpointHandlers;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class SoftwareEndpoints
{
    public static RouteGroupBuilder MapSoftwareEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("", CreateSoftwareEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Admin create new software"))
            .RequireAuthorization();
        return group;
    }
}