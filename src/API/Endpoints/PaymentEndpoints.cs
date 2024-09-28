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
                    "Handle payment return, called after payment is successful"
                )
            );

        group
            .MapGet("cancel", PaymentCancelEndpointHandler.Handle)
            .WithMetadata(
                new SwaggerOperationAttribute(
                    "Handle payment cancellation, called after payment is cancelled"
                )
            );

        group
            .MapPost("webhook", PaymentWebhookEndpointHandler.Handle)
            .WithMetadata(
                new SwaggerOperationAttribute(
                    "Handle payment webhook, called by PayOS to notify payment status"
                )
            );

        return group;
    }
}
