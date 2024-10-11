using API.Utils;
using Application.Responses.CartResponses;
using Application.UC_Cart.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.CartEndpointHandler;

public class AddProductToCartEndpointHandler
{
    public record AddProductToCartRequest(Guid productId);

    public static async Task<IResult> Handle(
        ISender sender,
        AddProductToCartRequest request,
        CancellationToken cancellationToken = default
    )
    {
        Result<AddProductToCartResponse> result = await sender.Send(
            new AddProductToCart.Command(ProductId: request.productId),
            cancellationToken
        );

        return result.Check();
    }
}
