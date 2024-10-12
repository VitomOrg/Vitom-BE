using API.EndpointHandlers.MaterialEndpointHandler;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class ModelMaterialEndpoints
{
    public static RouteGroupBuilder MapModelMaterialEndpoint(this RouteGroupBuilder group)
    {
        group
            .MapDelete("", DeleteModelMaterialEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Delete materials"))
            .RequireAuthorization();

        return group;
    }
}