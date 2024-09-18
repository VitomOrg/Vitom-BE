using API.Utils;
using Application.Responses.ProductResponses;
using Application.UC_Product.Commands;
using Ardalis.Result;
using Domain.Enums;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.ProductEndpointHandlers;

public class CreateProductEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender,
        [FromForm] LicenseEnum License,
        [FromForm] string Name,
        [FromForm] string Description,
        [FromForm] decimal Price,
        [FromForm] string DownloadUrl,
        [FromForm] Guid[] TypeIds,
        [FromForm] Guid[] SoftwareIds,
        IFormFileCollection Files,
        CancellationToken cancellationToken = default)
    {
        Result<CreateProductResponse> result = await sender.Send(new CreateProduct.Command(
            License: License,
            Name: Name,
            Description: Description,
            Price: Price,
            DownloadUrl: DownloadUrl,
            TypeIds: TypeIds,
            SoftwareIds: SoftwareIds,
            Images: Files
        ), cancellationToken);
        return result.Check();
    }
}
