using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public static class BlogImageGenerator
{
    public static BlogImage[] Generate(Blog[] blogs)
        => [.. new Faker<BlogImage>()
        .UseDateTimeReference(DateTime.UtcNow)
        .RuleFor(b => b.Id, f => f.Random.Uuid())
        .RuleFor(b => b.BlogId, f => f.PickRandom(blogs).Id)
        .RuleFor(b => b.Url, f => f.Image.PlaceImgUrl())
        .RuleFor(b => b.CreatedAt, f => f.Date.Past())
        .RuleFor(b => b.UpdatedAt, f => f.Random.Bool() ? f.Date.Past() : null!)
        .RuleFor(b => b.DeletedAt, f => f.Random.Bool() ? f.Date.Past() : null!)
        .Generate(10)
            ];
}