using API.Endpoints;
using API.Endpoints.CollectionEndpoints;

namespace API.Middlewares;

public static class MinimalAPIMiddleware
{
    public static WebApplication MapMinimalAPI(this WebApplication host)
    {
        host.MapGroup("collections").MapCollectionEndpoint().WithTags("Collections");
        host.MapGroup("users").MapUserEndpoint().WithTags("Users");
        host.MapGroup("softwares").MapSoftwareEndpoint().WithTags("Softwares");
        host.MapGroup("types").MapTypeEndpoint().WithTags("Types");
        host.MapGroup("reviews").MapReviewEndpoint().WithTags("Reviews");
        host.MapGroup("products").MapProductEndpoint().WithTags("Products");
        host.MapGroup("transactions").MapTransactionEndpoint().WithTags("Transactions");
        return host;
    }
}
