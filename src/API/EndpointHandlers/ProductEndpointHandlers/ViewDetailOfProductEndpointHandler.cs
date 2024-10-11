using API.Utils;
using Application.Responses.ProductResponses;
using Application.UC_Product.Queries;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.ProductEndpointHandlers;

public class ViewDetailOfProductEndpointHandler
{
    public static async Task<IResult> Handle(
        ISender sender,
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        Result<ProductDetailsResponse> result = await sender.Send(
            new ViewDetailOfProduct.Query(id),
            cancellationToken
        );

        return result.Check();
    }
}
