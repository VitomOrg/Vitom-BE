using API.EndpointHandlers.BlogEndpointHandler;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class BlogEndpoints
{
    public static RouteGroupBuilder MapBlogEndpoint(this RouteGroupBuilder group)
    {
        // GET
        group.MapGet("", GetBlogsEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get blogs"));
        group.MapGet("{id}", GetBlogByIdEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get blog by id"));
        group.MapGet("top", ViewTopBlogsEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get top blogs"));
        // POST
        group.MapPost("", CreateBlogEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Create new blog"))
            .RequireAuthorization().DisableAntiforgery();
        // PUT
        group.MapPut("{id}", UpdateBlogEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Update existing blog"))
            .RequireAuthorization().DisableAntiforgery();
        // 
        group.MapDelete("{id}", DeleteBlogEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Delete existing blog"))
            .RequireAuthorization();
        return group;
    }
}