using Application.Contracts;
using Application.Responses.CartResponses;
using Application.UC_Cart.Events;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Cart.Commands;

public class AddProductToCart
{
    public record Command(Guid ProductId) : IRequest<Result<AddProductToCartResponse>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser)
        : IRequestHandler<Command, Result<AddProductToCartResponse>>
    {
        public async Task<Result<AddProductToCartResponse>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            Product? product = await context
                .Products.AsNoTracking()
                .FirstOrDefaultAsync(
                    p => p.Id == request.ProductId && p.DeletedAt == null,
                    cancellationToken
                );

            if (product is null)
                return Result.NotFound("Product not found");

            // Check if the product is already in the cart
            CartItem? cartItem = context
                .CartItems.Include(ci => ci.Cart)
                .FirstOrDefault(ci =>
                    ci.ProductId == product.Id && ci.Cart.UserId == currentUser.User!.Id
                );

            if (cartItem is not null)
                return Result.Error("Product already in the cart");

            Cart? currentUserCart = await context
                .Carts
                .AsNoTracking()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == currentUser.User!.Id, cancellationToken);

            if (currentUserCart is null)
                return Result.Error("Cart not found");

            cartItem = new()
            {
                CartId = currentUserCart!.Id,
                ProductId = product.Id,
                PriceAtPurchase = product.Price
            };

            await context.CartItems.AddAsync(cartItem, cancellationToken);
            cartItem.AddDomainEvent(new CartUpdated.Event());
            await context.SaveChangesAsync(cancellationToken);
            return Result<AddProductToCartResponse>.Success(
                new(cartItem.Id, cartItem.ProductId, cartItem.PriceAtPurchase)
            );
        }
    }
}
