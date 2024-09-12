using API.EndpointHandlers.CollectionEndpointHandlers;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints.CollectionEndpoints;

public static class CollectionEndpoints
{
    public static RouteGroupBuilder MapCollectionEndpoint(this RouteGroupBuilder group)
    {
        group
            .MapPost("like", LikeCollectionEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Like a collection"))
            .RequireAuthorization();

        group
            .MapPost("dislike", DislikeCollectionEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Dislike a collection"))
            .RequireAuthorization();

        return group;
    }
}
