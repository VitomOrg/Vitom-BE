using Application.Mappers.ProductMappers;
using Application.Responses.CartResponses;
using Application.Responses.ProductResponses;
using Domain.Entities;

namespace Application.Mappers.CartMappers;

public static class CartItemResponseMapper
{
    public static CartItemResponse MapToCartItemResponse(this CartItem cartItem)
        => new(
            CartId: cartItem.CartId,
            Product: cartItem.Product.MapToProductDetailsResponse(),
            PriceAtPurchase: cartItem.PriceAtPurchase
        );
}