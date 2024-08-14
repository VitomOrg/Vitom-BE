using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace Persistence.DataGenerator;

public class CustomColorGenerator
{
    public static CustomColor[] Generate(Product[] products)
        => [.. new Faker<CustomColor>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e=>e.Id,f=>f.Random.Uuid())
            .RuleFor(e=>e.CreatedAt,f=>f.Date.Past())
            .RuleFor(e=>e.UpdatedAt,f=>f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.DeletedAt,(f,e) => f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.ProductId,f=>f.PickRandom(products).Id)
            .RuleFor(e=>e.Name,f=>f.Commerce.Color())
            .RuleFor(e=>e.Code,(f,e)=>f.Internet.Color())
            .Generate(100)
            .ToArray()
            // .DistinctBy(e=>new {e.ProductId,e.Name})
            // .ToArray()
        ];
}