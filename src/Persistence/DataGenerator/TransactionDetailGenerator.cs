using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public class TransactionDetailGenerator
{
    public static TransactionDetail[] Generate(Transaction[] transactions, Product[] products)
        => [.. new Faker<TransactionDetail>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e=>e.Id,f=>f.Random.Uuid())
            .RuleFor(e=>e.CreatedAt,f=>f.Date.Past())
            .RuleFor(e=>e.UpdatedAt,f=>f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.DeletedAt,(f,e) => f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.TransactionId,f=>f.PickRandom(transactions).Id)
            .RuleFor(e=>e.ProductId,f=>f.PickRandom(products).Id)
            .RuleFor(e=>e.PriceAtPurchase,f=>f.PickRandom<decimal>(1,9999999999))
            .Generate(100)
            .ToArray()
            .DistinctBy(e=>new {e.TransactionId,e.ProductId})
            .ToArray()
        ];
}