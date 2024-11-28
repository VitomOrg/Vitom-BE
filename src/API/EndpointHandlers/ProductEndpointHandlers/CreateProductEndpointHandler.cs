using API.Utils;
using Application.Responses.ProductResponses;
using Application.UC_Product.Commands;
using Ardalis.Result;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.ProductEndpointHandlers;

public class CreateProductEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender,
        CreateProductCommand command,
        // HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        // var form = await httpContext.Request.ReadFormAsync(cancellationToken);
        Result<CreateProductResponse> result = await sender.Send(new CreateProduct.Command(
            Name: command.name,
            Description: command.description,
            Price: command.price,
            TypeIds: command.typeIds,
            SoftwareIds: command.softwareIds,
            // Images: (List<IFormFile>)form.Files.GetFiles("Files"),
            // ModelMaterials: (List<IFormFile>)form.Files.GetFiles("ModelMaterialFiles"),
            Images: command.files,
            ModelMaterials: command.modelMaterialFiles,
            Fbx: command.fbx,
            Obj: command.obj,
            Glb: command.glb
        ), cancellationToken);
        return result.Check();
    }

    public record CreateProductCommand(
        LicenseEnum license,
        string name,
        string description,
        decimal price,
        Guid[] typeIds,
        Guid[] softwareIds,
        string[] files,
        string[] modelMaterialFiles,
        string fbx,
        string obj,
        string glb
    );
}