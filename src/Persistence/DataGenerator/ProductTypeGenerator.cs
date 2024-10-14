using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public class ProductTypeGenerator
{
    public static ProductType[] Generate(Product[] products, Domain.Entities.Type[] types)
        => [.. new Faker<ProductType>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e => e.Id, f => f.Random.Uuid())
            .RuleFor(e => e.CreatedAt, f => f.Date.Past())
            .RuleFor(e => e.UpdatedAt, f => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.DeletedAt, (f, e) => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.ProductId, f => f.PickRandom(products).Id)
            .RuleFor(e => e.TypeId, f => f.PickRandom(types).Id)
            .Generate(100)
            .ToArray()
            .DistinctBy(e => new { e.ProductId, e.TypeId })
            .ToArray()
        ];
}