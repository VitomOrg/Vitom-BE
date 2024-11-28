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
        Guid id,
        UpdateProductCommand command,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new UpdateProduct.Command(
            Id: id,
            Name: command.name,
            Description: command.description,
            Price: command.price,
            TypeIds: command.typeIds,
            SoftwareIds: command.softwareIds,
            Images: command.files,
            ModelMaterials: command.modelMaterialFiles,
            Fbx: command.fbx,
            Obj: command.obj,
            Glb: command.glb
        ), cancellationToken);
        return result.Check();
    }

    public record UpdateProductCommand(
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