using API.Utils;
using Application.UC_Product.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.ProductEndpointHandlers;

public class DeleteProductEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender, Guid Id, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new DeleteProduct.Command(Id: Id), cancellationToken);
        return result.Check();
    }
}
