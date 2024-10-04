using Application.Contracts;
using Application.Responses.CartResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Net.payOS.Types;

namespace Application.UC_Cart.Commands;

public class Checkout
{
    public record Command() : IRequest<Result<CheckoutResponse>>;

    public class Handler(
        IVitomDbContext context,
        CurrentUser currentUser,
        IPayOSServices payOSServices
    ) : IRequestHandler<Command, Result<CheckoutResponse>>
    {
        public async Task<Result<CheckoutResponse>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            Cart? cart = await context
                .Carts
                .AsSplitQuery()
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .Where(c => c.UserId == currentUser.User!.Id)
                .Where(c => c.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);

            if (cart is null)
                return Result.NotFound("Cart not found");

            int totalAmount = (int)cart.CartItems.Sum(ci => ci.PriceAtPurchase);

            // Add each product from CartItems to the productList
            List<ItemData> productList = [.. cart
                .CartItems
                .AsQueryable()
                .AsNoTracking()
                .Select(c => new ItemData(c.Product.Name, 1, (int)c.PriceAtPurchase))];

            CreatePaymentResult response = await payOSServices.CreateOrderAsync(totalAmount, productList);
            // Update OrderCode to db
            cart.OrderCode = response.orderCode;
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CheckoutResponse(response.checkoutUrl));
        }
    }
}
