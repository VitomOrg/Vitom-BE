using API.Utils;
using Application.Responses.CartResponses;
using Application.UC_Cart.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.CartEndpointHandler;

public class CheckoutEndpointHandler
{
    public static async Task<IResult> Handle(
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        Result<CheckoutResponse> result = await sender.Send(
            new Checkout.Command(),
            cancellationToken
        );

        return result.Check();
    }
}