using API.EndpointHandlers.TransactionEndpointHandlers;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class TransactionEndpoint
{
    public static RouteGroupBuilder MapTransactionEndpoint(this RouteGroupBuilder group)
    {
        group
            .MapGet("/user", FetchListOfTransactionEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get detail of transaction"))
            .RequireAuthorization();
        return group;
    }
}