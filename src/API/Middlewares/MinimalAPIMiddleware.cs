using API.Endpoints;

namespace API.Middlewares;
public static class MinimalAPIMiddleware
{
    public static WebApplication MapMinimalAPI(this WebApplication host)
    {
        host.MapGroup("users").MapUserEndpoint().WithTags("Users");
        host.MapGroup("softwares").MapSoftwareEndpoint().WithTags("Softwares");
        return host;
    }
}