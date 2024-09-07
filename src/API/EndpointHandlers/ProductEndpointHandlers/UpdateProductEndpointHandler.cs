using API.Utils;
using Application.UC_Product.Commands;
using Ardalis.Result;
using Domain.Enums;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.ProductEndpointHandlers;

public class UpdateProductEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender, Guid Id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new UpdateProduct.Command(
            Id: Id,
            Name: request.Name,
            Description: request.Description,
            Price: request.Price,
            DownloadUrl: request.DownloadUrl
        ), cancellationToken);
        return result.Check();
    }

    public record UpdateProductRequest(
        string Name,
        string Description,
        decimal Price,
        string DownloadUrl
    );
}
