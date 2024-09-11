using API.EndpointHandlers.CollectionEndpointHandlers;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints.CollectionEndpoints;

public static class CollectionEndpoints
{
    public static RouteGroupBuilder MapCollectionEndpoint(this RouteGroupBuilder group)
    {
        group
            .MapPost("", LikeCollectionEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Like a collection"))
            .RequireAuthorization();

        return group;
    }
}
