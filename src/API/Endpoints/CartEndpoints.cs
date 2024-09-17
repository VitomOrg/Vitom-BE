using API.EndpointHandlers.CartEndpointHandler;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class CartEndpoints
{
    public static RouteGroupBuilder MapCartEndpoint(this RouteGroupBuilder group)
    {
        group
            .MapPost("", AddProductToCartEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Add product to cart"))
            .RequireAuthorization();

        group
            .MapDelete("{ProductId}", DeleteProductFromCartEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Delete product from cart"))
            .RequireAuthorization();

        return group;
    }
}
