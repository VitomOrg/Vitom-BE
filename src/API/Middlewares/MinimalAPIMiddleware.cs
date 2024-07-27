using API.Endpoints;
using Npgsql.Replication;

namespace API.Middlewares;
public static class MinimalAPIMiddleware
{
    public static WebApplication MapMinimalAPI(this WebApplication host)
    {
        host.MapGroup("Users").MapUserEndpoint().WithTags("Users");

        return host;
    }
}