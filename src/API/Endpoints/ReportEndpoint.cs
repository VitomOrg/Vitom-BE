using API.EndpointHandlers.ReportEndpointHandler;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class ReportEndpoint
{
    public static RouteGroupBuilder MapReportEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("", FetchReportEndpointHandler.Handle)
            .WithMetadata(new SwaggerOperationAttribute("Get report"))
            .RequireAuthorization();

        return group;
    }
}