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
      
        return group;
    }
}
