using Application.Caches.Events;
using Application.Contracts;
using Application.Responses.CartResponses;
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
                .Where(p => p.DeletedAt == null)
                .FirstOrDefaultAsync(
                    p => p.Id == request.ProductId && p.DeletedAt == null,
                    cancellationToken
                );

            if (product is null)
                return Result.NotFound("Product not found");

            // Check if the product is already in the cart
            CartItem? cartItem = context
                .CartItems.Include(ci => ci.Cart)
                .Where(ci => ci.DeletedAt == null)
                .FirstOrDefault(ci =>
                    ci.ProductId == product.Id && ci.Cart.UserId == currentUser.User!.Id
                );

            if (cartItem is not null)
                return Result.Error("Product already in the cart");

            Cart? currentUserCart = await context
                .Carts
                .AsNoTracking()
                .Include(c => c.CartItems)
                .Where(c => c.DeletedAt == null)
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
            cartItem.AddDomainEvent(new EntityCreated.Event("cart"));
            await context.SaveChangesAsync(cancellationToken);
            return Result<AddProductToCartResponse>.Success(
                new(cartItem.Id, cartItem.ProductId, cartItem.PriceAtPurchase)
            );
        }
    }
}