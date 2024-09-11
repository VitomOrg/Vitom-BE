using API.EndpointHandlers.ProductEndpointHandlers;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoint(this RouteGroupBuilder group)
    {
        group
            .MapGet("/{Id}", ViewDetailOfProductEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get detail of product"));

        group
            .MapGet("/{productId}/reviews", FetchReviewsByProductEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get reviews of product"));

        group
            .MapGet("/list", FetchListOfProductsEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get list of products"));

        group
            .MapPost("", CreateProductEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Organization create new product"))
            .RequireAuthorization();

        group
            .MapPut("/{Id}", UpdateProductEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Organization update existing product"))
            .RequireAuthorization();

        group
            .MapDelete("/{Id}", DeleteProductEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Organization delete existing product"))
            .RequireAuthorization();

        return group;
    }
}
