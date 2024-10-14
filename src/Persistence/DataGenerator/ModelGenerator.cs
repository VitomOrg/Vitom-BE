using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public class ModelGenerator
{
    public static Model[] Generate(Product[] products)
        => [.. new Faker<Model>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            //base  enity
            .RuleFor(m => m.Glb, f => f.Image.LoremFlickrUrl())
            .RuleFor(m => m.Obj, f => f.Image.LoremFlickrUrl())
            .RuleFor(m => m.Fbx, f => f.Image.LoremFlickrUrl())
            .RuleFor(m => m.ProductId, f => f.PickRandom(products).Id)
            .Generate(100)
            .ToArray()
            .DistinctBy(m => m.ProductId)
            .ToArray()
        ];
}