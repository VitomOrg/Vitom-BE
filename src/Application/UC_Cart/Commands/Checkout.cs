using Application.Contracts;
using Application.Responses.CartResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;

namespace Application.UC_Cart.Commands;

public class Checkout
{
    public record Command() : IRequest<Result<CheckoutResponse>>;

    public class Handler(
        IVitomDbContext context,
        CurrentUser currentUser,
        IOptionsMonitor<PayOSSettings> _payOSSettings
    ) : IRequestHandler<Command, Result<CheckoutResponse>>
    {
        private readonly PayOSSettings payOSSettings = _payOSSettings.CurrentValue;

        public async Task<Result<CheckoutResponse>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            string clientId = payOSSettings.ClientId;
            string apiKey = payOSSettings.ApiKey;
            string checkSumKey = payOSSettings.CheckSumKey;
            string domain = "https://vitom.persiehomeserver.com";
            string returnUrl = $"{domain}/payment/return";
            string cancelUrl = $"{domain}/payment/cancel";

            PayOS? payOS = new(clientId, apiKey, checkSumKey);

            Cart? cart = await context
                .Carts.Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .Where(c => c.UserId == currentUser.User!.Id)
                .Where(c => c.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);

            if (cart is null)
                return Result.NotFound("Cart not found");

            int totalAmount = (int)cart.CartItems.Sum(ci => ci.PriceAtPurchase);

            // Add each product from CartItems to the productList
            List<ItemData> productList = cart
                .CartItems.Select(c => new ItemData(c.Product.Name, 1, (int)c.PriceAtPurchase))
                .ToList();

            int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            PaymentData paymentLinkRequest =
                new(
                    orderCode: orderCode,
                    amount: totalAmount,
                    description: $"Order {GenerateDescriptionCode()}",
                    items: productList,
                    returnUrl: returnUrl,
                    cancelUrl: cancelUrl,
                    expiredAt: (int?)DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds()
                );

            CreatePaymentResult response = await payOS.createPaymentLink(paymentLinkRequest);
            cart.OrderCode = orderCode;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CheckoutResponse(response.checkoutUrl));
        }

        // Generate a random string for the description code
        private static readonly Random random = new();
        private static int sequence = 0;

        public static string GenerateDescriptionCode()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            string randomPart =
                new(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());

            int sequentialPart = Interlocked.Increment(ref sequence) % 10000;

            return $"{randomPart}{sequentialPart:D4}";
        }
    }
}
