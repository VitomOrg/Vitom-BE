using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Payment.Commands;

public class ProcessPaymentWebhook
{
    public record Command(PaymentWebhookData WebhookData) : IRequest<Result>;

    public class Handler(IVitomDbContext context) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!(request.WebhookData.Success && request.WebhookData.Data.Code == "00"))
                return Result.Success();

            // Process successful payment
            Cart? cart = await context
                .Carts.Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(
                    c => c.Id == Guid.Parse(request.WebhookData.Data.PaymentLinkId),
                    cancellationToken
                );

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

            context.TransactionDetails.AddRange(transactionDetails);

            context.CartItems.RemoveRange(cart.CartItems);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
