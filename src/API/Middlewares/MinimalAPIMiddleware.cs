using API.Endpoints;

namespace API.Middlewares;

public static class MinimalAPIMiddleware
{
    public static WebApplication MapMinimalAPI(this WebApplication host)
    {
        host.MapGroup("carts").MapCartEndpoint().WithTags("Carts");
        host.MapGroup("collections").MapCollectionEndpoint().WithTags("Collections");
        host.MapGroup("users").MapUserEndpoint().WithTags("Users");
        host.MapGroup("softwares").MapSoftwareEndpoint().WithTags("Softwares");
        host.MapGroup("types").MapTypeEndpoint().WithTags("Types");
        host.MapGroup("reviews").MapReviewEndpoint().WithTags("Reviews");
        host.MapGroup("products").MapProductEndpoint().WithTags("Products");
        host.MapGroup("transactions").MapTransactionEndpoint().WithTags("Transactions");
        host.MapGroup("blogs").MapBlogEndpoint().WithTags("Blogs");
        host.MapGroup("payment").MapPaymentEndpoint().WithTags("Payments");
        host.MapGroup("report").MapReportEndpoint().WithTags("Reports");
        return host;
    }
}
