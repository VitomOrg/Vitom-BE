using API.Utils;
using Application.UC_Material;
using Ardalis.Result;
using MediatR;

namespace API.EndpointHandlers.MaterialEndpointHandler;

public class DeleteModelMaterialEndpointHandler
{
    public static async Task<Microsoft.AspNetCore.Http.IResult> Handle(ISender sender,
        Guid productId,
        Guid[] materialIds,
        CancellationToken cancellationToken = default
    )
    {
        Result result = await sender.Send(
            new DeleteMaterial.Command(productId, materialIds),
            cancellationToken
        );
        return result.Check();
    }
}