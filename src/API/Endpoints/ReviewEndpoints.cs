using API.EndpointHandlers.ProductEndpointHandlers;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class ReviewEndpoints
{
    public static RouteGroupBuilder MapReviewEndpoint(this RouteGroupBuilder group)
    {
        group
            .MapGet("/product/{productId}", FetchReviewsByProductEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get reviews of product"));

        return group;
    }
}