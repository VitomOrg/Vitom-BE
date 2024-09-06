using API.EndpointHandlers.TypeEndpointHandlers;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class TypeEndpoints
{
    public static RouteGroupBuilder MapTypeEndpoint(this RouteGroupBuilder group)
    {
        group
            .MapGet("", ViewListOfTypeEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get types"));

        group
            .MapPost("", CreateTypeEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Admin create new type"))
            .RequireAuthorization();

        group
            .MapPut("/{id}", UpdateTypeEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Admin update existing type"))
            .RequireAuthorization();

        group
            .MapDelete("/{id}", DeleteTypeEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Admin delete existing type"))
            .RequireAuthorization();

        return group;
    }
}
