using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public class ModelMaterialGenerator
{
    public static ModelMaterial[] Generate(Product[] products)
        => [.. new Faker<ModelMaterial>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e=>e.Id,f=>f.Random.Uuid())
            .RuleFor(e=>e.CreatedAt,f=>f.Date.Past())
            .RuleFor(e=>e.UpdatedAt,f=>f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.DeletedAt,(f,e) => f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.ProductId,f=>f.PickRandom(products).Id)
            .RuleFor(e=>e.Url,f=>f.Image.LoremFlickrUrl())
            .Generate(100)
            .ToArray()
            .DistinctBy(e=>new {e.ProductId,e.Url})
        ];
}