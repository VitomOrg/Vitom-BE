using API.EndpointHandlers.DeleteProductImageHandler;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class ProductImageEndpoints
{
    public static RouteGroupBuilder MapImageEndpoint(this RouteGroupBuilder group)
    {
        group
            .MapDelete("", DeleteProductImageEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Delete product image"))
            .RequireAuthorization();

        return group;
    }
}