using API.Utils;
using Application.UC_Payment.Commands;
using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.PaymentEndpointHandler;

public class PaymentReturnEndpointHandler
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
        Result result = await sender.Send(
            new ProcessPaymentReturn.Command(code, id, cancel, status, orderCode),
            cancellationToken
        );

        return result.Check();
    }
}