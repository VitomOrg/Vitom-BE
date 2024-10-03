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
        [FromForm] IFormFileCollection Files, //param for upload file in swagger
        [FromForm] IFormFileCollection ModelMaterialFiles, // param for upload model material in swagger
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var form = await httpContext.Request.ReadFormAsync(cancellationToken);
        Result result = await sender.Send(new UpdateProduct.Command(
            Id: Id,
            License: License,
            Name: Name,
            Description: Description,
            Price: Price,
            DownloadUrl: DownloadUrl,
            TypeIds: TypeIds,
            SoftwareIds: SoftwareIds,
            Images: (List<IFormFile>)form.Files.GetFiles("Files"),
            ModelMaterials: (List<IFormFile>)form.Files.GetFiles("ModelMaterialFiles")
        ), cancellationToken);
        return result.Check();
    }
}
