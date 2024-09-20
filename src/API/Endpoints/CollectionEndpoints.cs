using API.EndpointHandlers.CollectionEndpointHandlers;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints.CollectionEndpoints;

public static class CollectionEndpoints
{
    public static RouteGroupBuilder MapCollectionEndpoint(this RouteGroupBuilder group)
    {
        group
            .MapPut("like", LikeCollectionEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Like a collection"))
            .RequireAuthorization();

        group
            .MapGet("", ViewAllPublicCollectionEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("View all public collections"));

        group
            .MapGet("liked", ViewLikedCollectionEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("View liked collections"))
            .RequireAuthorization();

        group.MapPut("", UpdateCollectionEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Update a collection"))
            .RequireAuthorization();

        return group;
    }
}
