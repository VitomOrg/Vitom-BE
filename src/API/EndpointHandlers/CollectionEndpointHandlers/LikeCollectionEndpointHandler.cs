using API.Utils;
using Application.Responses.CollectionResponses;
using Application.UC_Collection.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.CollectionEndpointHandlers;

public class LikeCollectionEndpointHandler
{
    public record LikeCollectionRequest(Guid CollectionId);

    public static async Task<IResult> Handle(
        ISender sender,
        LikeCollectionRequest request,
        CancellationToken cancellationToken = default
    )
    {
        Result<LikeCollectionResponse> result = await sender.Send(
            new LikedCollection.Command(CollectionId: request.CollectionId),
            cancellationToken
        );

        return result.Check();
    }
}