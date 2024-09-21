using API.Utils;
using Application.Responses.CartResponses;
using Application.UC_Cart.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.CartEndpointHandler;

public class CheckoutEndpointHandler
{
    public record CheckoutEndpointHandlerRequest(Guid CartId);

    public static async Task<IResult> Handle(
        ISender sender,
        CheckoutEndpointHandlerRequest request,
        CancellationToken cancellationToken = default
    )
    {
        Result<CheckoutResponse> result = await sender.Send(
            new Checkout.Command(CartId: request.CartId),
            cancellationToken
        );

        return result.Check();
    }
}
