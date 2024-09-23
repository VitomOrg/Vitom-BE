using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Payment.Commands;

public class ProcessPaymentCancel
{
    public record Command(string Code, string Id, bool Cancel, string Status, int OrderCode)
        : IRequest<Result>;

    public class Handler(IVitomDbContext context) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            Cart? cart = await context
                .Carts.Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.OrderCode == request.OrderCode, cancellationToken);

            if (cart is null)
                return Result.NotFound("Cart not found");

            context.CartItems.RemoveRange(cart.CartItems);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
