using API.Utils;
using Application.UC_Image.Commands;
using Ardalis.Result;
using MediatR;

namespace API.EndpointHandlers.DeleteProductImageHandler;

public class DeleteProductImageEndpointHandler
{
    public static async Task<Microsoft.AspNetCore.Http.IResult> Handle(ISender sender,
        Guid productId,
        Guid[] imageIds,
        CancellationToken cancellationToken = default
    )
    {
        Result result = await sender.Send(
            new DeleteImage.Command(productId, imageIds),
            cancellationToken
        );
        return result.Check();
    }
}