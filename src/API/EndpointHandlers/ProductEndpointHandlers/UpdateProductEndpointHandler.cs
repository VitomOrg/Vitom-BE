using API.Utils;
using Application.UC_Product.Commands;
using Ardalis.Result;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.ProductEndpointHandlers;

public class UpdateProductEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender,
        [FromForm] Guid Id,
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
        Result result = await sender.Send(new UpdateProduct.Command(
            Id: Id,
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
