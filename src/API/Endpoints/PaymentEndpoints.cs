using API.EndpointHandlers.PaymentEndpointHandler;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints;

public static class PaymentEndpoints
{
    public static RouteGroupBuilder MapPaymentEndpoint(this RouteGroupBuilder group)
    {
        group
            .MapGet("return", PaymentReturnEndpointHandler.Handle)
            .WithMetadata(
                new SwaggerOperationAttribute(
                    "Handle payment return, called after payment is successful (by PayOS)"
                )
            );

        group
            .MapGet("cancel", PaymentCancelEndpointHandler.Handle)
            .WithMetadata(
                new SwaggerOperationAttribute(
                    "Handle payment cancellation, called after payment is cancelled (by PayOS)"
                )
            );

        return group;
    }
}
