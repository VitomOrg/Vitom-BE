using Application.Responses.CartResponses;

namespace Application.Mappers.CartMappers;

public static class AddProductToCartResponseMapper
{
    public static AddProductToCartResponse Map(
        Guid cartId,
        Guid productId,
        decimal priceAtPurchase
    ) => new(cartId, productId, priceAtPurchase);
}
