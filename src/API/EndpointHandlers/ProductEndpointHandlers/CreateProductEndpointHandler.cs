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
    public record CreateProductRequest(
        LicenseEnum license,
        string name,
        string description,
        decimal price,
        Guid[] typeIds,
        Guid[] softwareIds,
        List<Stream> files, //param for upload file in swagger
        List<Stream> modelMaterialFiles, // param for upload model material in swagger
        Stream fbx,
        Stream obj,
        Stream glb
    );
    public static async Task<IResult> Handle(ISender sender,
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        // var form = await httpContext.Request.ReadFormAsync(cancellationToken);
        Result<CreateProductResponse> result = await sender.Send(new CreateProduct.Command(
            License: request.license,
            Name: request.name,
            Description: request.description,
            Price: request.price,
            TypeIds: request.typeIds,
            SoftwareIds: request.softwareIds,
            Images: request.files,
            ModelMaterials: request.modelMaterialFiles,
            Fbx: request.fbx,
            Obj: request.obj,
            Glb: request.glb
        ), cancellationToken);
        return result.Check();
    }
}