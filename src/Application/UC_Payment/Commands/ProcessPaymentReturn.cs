using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Payment.Commands;

public class ProcessPaymentReturn
{
    public record Command(string Code, string Id, bool Cancel, string Status, int OrderCode)
        : IRequest<Result>;

    public class Handler(IVitomDbContext context) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.Status != "PAID" || request.Cancel)
                return Result.Success();

            // Process successful payment
            var cart = await context
                .Carts.Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.OrderCode == request.OrderCode, cancellationToken);

            if (cart is null)
                return Result.NotFound("Cart not found");

            // Create transaction
            Transaction transaction =
                new()
                {
                    UserId = cart.UserId,
                    TotalAmount = cart.CartItems.Sum(ci => ci.PriceAtPurchase),
                    PaymentMethod = Domain.Enums.PaymentMethodEnum.PayOS,
                    TransactionStatus = Domain.Enums.TransactionStatusEnum.Completed
                };

            context.Transactions.Add(transaction);

            // Create transaction details
            var transactionDetails = cart
                .CartItems.Select(cartItem => new TransactionDetail
                {
                    TransactionId = transaction.Id,
                    ProductId = cartItem.ProductId,
                    PriceAtPurchase = cartItem.PriceAtPurchase
                })
                .ToList();

            // Add each product from CartItems to the user library
            var userLibrary = cart
                .CartItems.Select(cartItem => new UserLibrary
                {
                    UserId = cart.UserId,
                    ProductId = cartItem.ProductId
                })
                .ToList();

            // Select each product from productId in CartItems to update total purchase count
            var products = await context
                .Products.Where(p => cart.CartItems.Select(ci => ci.ProductId).Contains(p.Id))
                .Where(p => p.DeletedAt == null)
                .ToListAsync(cancellationToken);

            foreach (var product in products)
            {
                product.TotalPurchases += cart
                    .CartItems.Where(ci => ci.ProductId == product.Id)
                    .Count();
            }

            context.TransactionDetails.AddRange(transactionDetails);
            context.UserLibrarys.AddRange(userLibrary);

            context.CartItems.RemoveRange(cart.CartItems);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
