using API.EndpointHandlers.BlogEndpointHandler;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class BlogEndpoints
{
    public static RouteGroupBuilder MapBlogEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("", CreateBlogEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Create new blog"))
            .RequireAuthorization().DisableAntiforgery();

        return group;
    }
}
