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
        [FromForm] Guid id,
        [FromForm] LicenseEnum license,
        [FromForm] string name,
        [FromForm] string description,
        [FromForm] decimal price,
        [FromForm] Guid[] typeIds,
        [FromForm] Guid[] softwareIds,
        [FromForm] IFormFileCollection files, //param for upload file in swagger
        [FromForm] IFormFileCollection modelMaterialFiles, // param for upload model material in swagger
        IFormFile fbx,
        IFormFile obj,
        IFormFile glb,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var form = await httpContext.Request.ReadFormAsync(cancellationToken);
        Result result = await sender.Send(new UpdateProduct.Command(
            Id: id,
            License: license,
            Name: name,
            Description: description,
            Price: price,
            TypeIds: typeIds,
            SoftwareIds: softwareIds,
            Images: (List<IFormFile>)form.Files.GetFiles("Files"),
            ModelMaterials: (List<IFormFile>)form.Files.GetFiles("ModelMaterialFiles"),
            Fbx: fbx,
            Obj: obj,
            Glb: glb
        ), cancellationToken);
        return result.Check();
    }
}
