using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public class CartItemGenerator
{
    public static CartItem[] Generate(Cart[] carts, Product[] products) =>
        [
            .. new Faker<CartItem>()
                .UseSeed(1)
                .UseDateTimeReference(DateTime.UtcNow)
                // base entity
                .RuleFor(e => e.Id, f => f.Random.Uuid())
                .RuleFor(e => e.CreatedAt, f => f.Date.Past())
                .RuleFor(e => e.UpdatedAt, f => f.Random.Bool() ? f.Date.Past() : null!)
                .RuleFor(e => e.DeletedAt, (f, e) => f.Random.Bool() ? f.Date.Past() : null!)
                .RuleFor(e => e.CartId, f => f.PickRandom(carts).Id)
                .RuleFor(e => e.ProductId, f => f.PickRandom(products).Id)
                .RuleFor(
                    e => e.PriceAtPurchase,
                    (f, e) => products.FirstOrDefault(p => p.Id.Equals(e.ProductId))!.Price
                )
                .Generate(100)
                .ToArray()
                .DistinctBy(e => new { e.CartId, e.ProductId })
                .ToArray()
        ];
}