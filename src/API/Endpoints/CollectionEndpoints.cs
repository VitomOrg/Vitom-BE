using API.EndpointHandlers.CollectionEndpointHandlers;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class CollectionEndpoints
{
    public static RouteGroupBuilder MapCollectionEndpoint(this RouteGroupBuilder group)
    {
        // GET
        group
            .MapGet("", ViewAllPublicCollectionEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("View all public collections"));
        group
            .MapGet("liked", ViewLikedCollectionEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("View liked collections"))
            .RequireAuthorization();
        // POST
        group.MapPost("", CreateCollectionEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Create a new collection"))
            .RequireAuthorization();
        // PUT
        group
            .MapPut("like", LikeCollectionEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Like a collection"))
            .RequireAuthorization();
        group.MapPut("", UpdateCollectionEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Update a collection"))
            .RequireAuthorization();
        // DELETE
        group.MapDelete("{id}", DeleteCollectionEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Delete a collection"))
            .RequireAuthorization();
        return group;
    }
}
