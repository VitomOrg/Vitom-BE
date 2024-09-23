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
            ProductId: request.ProductId,
            Rating: request.Rating,
            Content: request.Content
        ), cancellationToken);
        return result.Check();
    }

    public record CreateReviewRequest
    {
        public Guid ProductId { get; init; }
        public int Rating { get; init; }
        public string Content { get; init; } = "";
    }
}