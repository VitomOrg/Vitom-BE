using API.Utils;
using Application.Responses.CollectionResponses;
using Application.UC_Collection.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.CollectionEndpointHandlers;

public class UpdateCollectionEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender,
        Guid Id,
        UpdateCollectionRequest request,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new UpdateCollection.Command(
                Id: Id,
                Name: request.Name,
                Description: request.Description,
                IsPublic: request.IsPublic
            ), cancellationToken);

        return result.Check();
    }

    public record UpdateCollectionRequest(
        string Name = "",
        string Description = "",
        bool IsPublic = false
    );
}