using API.Utils;
using Application.Responses.LikeProductResponses;
using Application.UC_LikeProduct.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.LikeProductEndpointHandler;

public class CreateLikeProductEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender,
        Guid productId,
        CancellationToken cancellationToken = default
    )
    {
        Result<CreateLikeProductResponse> result = await sender.Send(
            new CreateLikeProduct.Command(productId),
            cancellationToken
        );
        return result.Check();
    }
}