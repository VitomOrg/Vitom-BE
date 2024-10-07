using API.Utils;
using Application.Responses.ReviewResponses;
using Application.UC_Review.Commands;
using Ardalis.Result;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace API.EndpointHandlers.ReviewEndpointHandler;

public class CreateReviewEndpointHandler
{
    public static async Task<IResult> Handle(ISender sender, CreateReviewRequest request, CancellationToken cancellationToken = default)
    {
        Result<CreateReviewResponse> result = await sender.Send(new CreateReview.Command(
            ProductId: request.productId,
            Rating: request.rating,
            Content: request.content
        ), cancellationToken);
        return result.Check();
    }

    public record CreateReviewRequest
    {
        public Guid productId { get; init; }
        public int rating { get; init; }
        public string content { get; init; } = "";
    }
}