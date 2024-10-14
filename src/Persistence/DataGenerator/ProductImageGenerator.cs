using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public class ProductImageGenerator
{
    public static ProductImage[] Generate(Product[] products)
        => [.. new Faker<ProductImage>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e => e.Id, f => f.Random.Uuid())
            .RuleFor(e => e.CreatedAt, f => f.Date.Past())
            .RuleFor(e => e.UpdatedAt, f => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.DeletedAt, (f, e) => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.ProductId, f => f.PickRandom(products).Id)
            .RuleFor(e => e.Url, f => f.Image.PicsumUrl())
            .Generate(2000)
            .ToArray()
            .DistinctBy(e => new { e.ProductId, e.Url })
        ];
}