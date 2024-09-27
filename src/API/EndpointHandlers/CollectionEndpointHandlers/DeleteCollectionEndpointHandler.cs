using API.Utils;
using Application.UC_Collection.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.CollectionEndpointHandlers;

public class DeleteCollectionEndpointHandler
{

    public static async Task<IResult> Handle(
        ISender sender,
        Guid Id,
        CancellationToken cancellationToken = default
    )
    {
        Result result = await sender.Send(new DeleteCollection.Command(Id: Id), cancellationToken);
        return result.Check();
    }
}