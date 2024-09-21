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

        group
            .MapPost("checkout", CheckoutEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Checkout cart"))
            .RequireAuthorization();

        return group;
    }
}
