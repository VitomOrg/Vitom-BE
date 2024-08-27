using API.EndpointHandlers.ProductEndpointHandlers;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{productId}/reviews",FetchReviewsByProductEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get reviews of product"));
        return group;
    }
}