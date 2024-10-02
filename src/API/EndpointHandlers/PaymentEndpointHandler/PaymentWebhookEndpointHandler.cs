using Application.UC_Payment.Commands;
using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.PaymentEndpointHandler;

public class PaymentWebhookEndpointHandler
{
    public static async Task<IResult> Handle(
        [FromBody] WebhookType webhookBody,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        return Results.Ok();
        Result result = await sender.Send(
            new ProcessPaymentWebhook.Command(webhookBody),
            cancellationToken
        );

        return result.IsSuccess
            ? Results.Ok(new { success = true })
            : Results.Ok(new { success = false, message = result.Errors });
    }
}
