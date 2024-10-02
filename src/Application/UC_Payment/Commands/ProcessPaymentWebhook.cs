using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;

namespace Application.UC_Payment.Commands;

public class ProcessPaymentWebhook
{
    public record Command(WebhookType WebhookData) : IRequest<Result>;

    public class Handler(IVitomDbContext context, IOptionsMonitor<PayOSSettings> payOSSettings) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            string clientId = payOSSettings.CurrentValue.ClientId;
            string apiKey = payOSSettings.CurrentValue.ApiKey;
            string checkSumKey = payOSSettings.CurrentValue.CheckSumKey;
            PayOS payOS = new(clientId, apiKey, checkSumKey);
            WebhookData webhookData = payOS.verifyPaymentWebhookData(request.WebhookData);
            // if (!(WebhookData.Success && request.WebhookData.Data.Code == "00"))
            //     return Result.Success();

            // Process successful payment
            Cart? cart = await context
                .Carts.Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(
                    c => c.Id == Guid.Parse(webhookData.paymentLinkId),
                    cancellationToken
                );

            if (cart is null)
                return Result.NotFound("Cart not found");

            // Create transaction
            Domain.Entities.Transaction transaction =
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
