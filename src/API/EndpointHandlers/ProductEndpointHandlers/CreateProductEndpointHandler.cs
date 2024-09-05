using API.Utils;
using Application.Responses.ProductResponses;
using Application.UC_Product.Commands;
using Ardalis.Result;
using Domain.Enums;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.ProductEndpointHandlers;

public class CreateProductEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender, CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        Result<CreateProductResponse> result = await sender.Send(new CreateProduct.Command(
            License: request.License,
            Name: request.Name,
            Description: request.Description,
            Price: request.Price,
            DownloadUrl: request.DownloadUrl
        ), cancellationToken);
        return result.Check();
    }

    public record CreateProductRequest(
        LicenseEnum License,
        string Name,
        string Description,
        decimal Price,
        string DownloadUrl
    );
}
