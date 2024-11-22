using API.EndpointHandlers.FileEndpointHandler;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class FileEndpoint
{
    public static RouteGroupBuilder MapFileEndpoint(this RouteGroupBuilder group)
    {
        group
            .MapPost("", UploadFileEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Upload file (File enum 1: Blog, 2: Product, 3: Model, 4: ModelMaterial)"))
            .RequireAuthorization()
            .DisableAntiforgery();

        return group;
    }
}