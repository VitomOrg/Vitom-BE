using System.Collections.Generic;
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
        IOptionsMonitor<PayOSSettings> _payOSSettings,
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
            List<ItemData> productList = [];
            foreach (var cartItem in cart.CartItems)
            {
                string productName = cartItem.Product.Name;
                int quantity = 1;
                int price = (int)cartItem.PriceAtPurchase;

                productList.Add(new ItemData(productName, quantity, price));
            }

            string domain = $"order/success/";

            var paymentLinkRequest = new PaymentData(
                orderCode: int.Parse(DateTimeOffset.Now.ToString("ffffff")),
                amount: totalAmount,
                description: $"Đon hang {GenerateDescriptionCode()}",
                items: productList,
                returnUrl: domain,
                cancelUrl: domain
            );

            var response = await payOS.createPaymentLink(paymentLinkRequest);

            return Result<CheckoutResponse>.Success(new(response.checkoutUrl));
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
