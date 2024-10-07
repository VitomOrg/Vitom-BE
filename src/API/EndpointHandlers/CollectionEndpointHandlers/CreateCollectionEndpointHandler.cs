using API.Utils;
using Application.Responses.CollectionResponses;
using Application.UC_Collection.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.CollectionEndpointHandlers;

public class CreateCollectionEndpointHandler
{
    public record CreateCollectionRequest(
        string name = "",
        string description = "",
        bool isPublic = true
    );
    public static async Task<IResult> Handle(
        ISender sender,
        CreateCollectionRequest request,
        CancellationToken cancellationToken = default
    )
    {
        Result<CreateCollectionResponse> result = await sender.Send(
            new CreateCollection.Command(
                Name: request.name,
                Description: request.description,
                IsPublic: request.isPublic
            ),
            cancellationToken
        );
        return result.Check();
    }
}