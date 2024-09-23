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
        HttpContext httpContext,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        string baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";

        Result<CheckoutResponse> result = await sender.Send(
            new Checkout.Command(baseUrl),
            cancellationToken
        );

        return result.Check();
    }
}
