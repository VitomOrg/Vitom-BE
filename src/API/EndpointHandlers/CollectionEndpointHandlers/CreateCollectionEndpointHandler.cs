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
        string Name = "",
        string Description = "",
        bool IsPublic = true
    );
    public static async Task<IResult> Handle(
        ISender sender,
        CreateCollectionRequest request,
        CancellationToken cancellationToken = default
    )
    {
        Result<CreateCollectionResponse> result = await sender.Send(
            new CreateCollection.Command(
                Name: request.Name,
                Description: request.Description,
                IsPublic: request.IsPublic
            ),
            cancellationToken
        );
        return result.Check();
    }
}