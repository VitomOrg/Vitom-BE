namespace Application.Responses.CartResponses;

public record AddProductToCartResponse(
    Guid CartId,
    Guid ProductId,
    decimal PriceAtPurchase
);
