using Application.UC_Payment.Commands;
using Ardalis.Result;
using Domain.Primitives;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
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

        Result result = await sender.Send(
            new ProcessPaymentWebhook.Command(webhookBody),
            cancellationToken
        );

        return result.IsSuccess
            ? Results.Ok(new { success = true })
            : Results.BadRequest(new { success = false, message = result.Errors });
    }
}
