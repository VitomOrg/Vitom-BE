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
    public record Command(Guid CartId) : IRequest<Result<CheckoutResponse>>;

    public class Handler(IVitomDbContext context, IOptionsMonitor<PayOSSettings> _payOSSettings)
        : IRequestHandler<Command, Result<CheckoutResponse>>
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
                .Where(c => c.Id == request.CartId)
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
                description: "Thanh toan don hang",
                items: productList,
                returnUrl: domain,
                cancelUrl: domain
            );

            var response = await payOS.createPaymentLink(paymentLinkRequest);

            return Result<CheckoutResponse>.Success(new(response.checkoutUrl));
        }
    }
}
