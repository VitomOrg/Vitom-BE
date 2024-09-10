namespace Application.Responses.ProductResponses;

public record FetchProductsForEachSoftwareResponse(
    string Software,
    IEnumerable<ProductDetailsResponse> Products
);