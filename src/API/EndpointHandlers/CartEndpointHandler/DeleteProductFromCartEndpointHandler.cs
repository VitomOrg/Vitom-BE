using API.Utils;
using Application.UC_Cart.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.CartEndpointHandler;

public class DeleteProductFromCartEndpointHandler
{
    public static async Task<IResult> Handle(
        ISender sender,
        Guid productId,
        CancellationToken cancellationToken = default
    )
    {
        Result result = await sender.Send(
            new DeleteProductFromCart.Command(productId),
            cancellationToken
        );

        return result.Check();
    }
}
