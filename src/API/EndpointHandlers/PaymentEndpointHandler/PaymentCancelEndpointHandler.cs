using MediatR;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.PaymentEndpointHandler;

public class PaymentCancelEndpointHandler
{
    public static async Task<IResult> Handle(
        [FromQuery] string code,
        [FromQuery] string id,
        [FromQuery] bool cancel,
        [FromQuery] string status,
        [FromQuery] int orderCode,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        // Note: This is not the actual implementation, but a placeholder

        // Result result = await sender.Send(
        //     new ProcessPaymentCancel.Command(code, id, cancel, status, orderCode),
        //     cancellationToken
        // );
        await Task.Delay(0, cancellationToken);
        return Results.Redirect("/payment-cancelled"); // Redirect to a cancellation page
    }
}
