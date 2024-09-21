using Application.Responses.ProductResponses;

namespace Application.Responses.CartResponses;

public record CartItemResponse
(
    Guid CartId,
    ProductDetailsResponse Product,
    decimal PriceAtPurchase
);