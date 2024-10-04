using Application.Responses.ReviewResponses;

namespace Application.Responses.RatingResponse.cs;

public record RatingWithReviewResponse
(
    int Key,
    IEnumerable<ReviewDetailsResponse> ReviewDetails
);