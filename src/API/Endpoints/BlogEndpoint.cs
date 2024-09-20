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
        // POST
        group.MapPost("", CreateBlogEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Create new blog"))
            .RequireAuthorization().DisableAntiforgery();
        // PUT
        group.MapPut("{Id}", UpdateBlogEndpointHandler.Handle)
        .WithMetadata(new SwaggerOperationAttribute("Update existing blog"))
        .RequireAuthorization().DisableAntiforgery();

        return group;
    }
}
