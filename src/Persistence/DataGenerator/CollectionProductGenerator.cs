using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public class CollectionProductGenerator
{
    public static CollectionProduct[] Generate(Collection[] collections, Product[] products)
        => [.. new Faker<CollectionProduct>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e => e.Id, f => f.Random.Uuid())
            .RuleFor(e => e.CreatedAt, f => f.Date.Past())
            .RuleFor(e => e.UpdatedAt, f => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.DeletedAt, (f, e) => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.CollectionId, f => f.PickRandom(collections).Id)
            .RuleFor(e => e.ProductId, f => f.PickRandom(products).Id)
            .Generate(100)
            .ToArray()
            .DistinctBy(e => new { e.CollectionId, e.ProductId })
            .ToArray()
        ];
}