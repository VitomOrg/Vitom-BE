using API.EndpointHandlers.ProductEndpointHandlers;
using API.EndpointHandlers.UserEndpointHandlers;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoint(this RouteGroupBuilder group)
    {
        group
            .MapGet("/{id}", ViewDetailOfProductEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get detail of product"));

        group
            .MapGet("/list", FetchListOfProductsEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get list of products"));

        group.MapGet("/user", FetchLikedProductByUserIdEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get product user liked"))
            .RequireAuthorization();

        group
            .MapPost("", CreateProductEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Organization create new product"))
            .RequireAuthorization()
            .DisableAntiforgery();

        group
            .MapPut("/{id}", UpdateProductEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Organization update existing product"))
            .RequireAuthorization()
            .DisableAntiforgery(); ;

        group
            .MapDelete("/{id}", DeleteProductEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Organization delete existing product"))
            .RequireAuthorization()
            .DisableAntiforgery();

        return group;
    }
}
