using API.Utils;
using Application.UC_Collection.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.CollectionEndpointHandlers;

public class DislikeCollectionEndpointHandler
{
    public record DislikeCollectionRequest(Guid CollectionId);

    public static async Task<IResult> Handle(
        ISender sender,
        DislikeCollectionRequest request,
        CancellationToken cancellationToken = default
    )
    {
        Result result = await sender.Send(
            new DislikedCollection.Command(CollectionId: request.CollectionId),
            cancellationToken
        );

        return result.Check();
    }
}
