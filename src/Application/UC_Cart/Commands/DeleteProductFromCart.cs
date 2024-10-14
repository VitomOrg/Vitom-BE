using Application.Caches.Events;
using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Cart.Commands;

public class DeleteProductFromCart
{
    public record Command(Guid ProductId) : IRequest<Result>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            Product? product = await context
                .Products.AsNoTracking()
                .Where(p => p.DeletedAt == null)
                .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

            if (product is null)
                return Result.NotFound("Product not found");

            CartItem? cartItem = await context.CartItems.FirstOrDefaultAsync(
                ci => ci.ProductId == product.Id && ci.Cart.UserId == currentUser.User!.Id,
                cancellationToken
            );

            if (cartItem is null)
                return Result.NotFound("Product not found in the cart");

            context.CartItems.Remove(cartItem);
            cartItem.AddDomainEvent(new EntityRemove.Event("cart"));
            await context.SaveChangesAsync(cancellationToken);
            return Result.NoContent();
        }
    }
}