using API.EndpointHandlers.ProductEndpointHandlers;
using API.EndpointHandlers.SoftwareEndpointHandlers;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class SoftwareEndpoints
{
    public static RouteGroupBuilder MapSoftwareEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("", ViewListOfSoftwaresEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get softwares"));
        group.MapGet("/products", FetchProductsForEachSoftwareEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get products for each software"));
        group.MapPost("", CreateSoftwareEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Admin create new software"))
            .RequireAuthorization();
        group.MapPut("/{id}", UpdateSoftwareEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Admin update existing software"))
            .RequireAuthorization();
        group.MapDelete("/{id}", DeleteSoftwareEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Admin delete existing software"))
            .RequireAuthorization();
        return group;
    }
}