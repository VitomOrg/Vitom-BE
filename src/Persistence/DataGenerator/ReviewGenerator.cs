using Bogus;
using Domain.Entities;
using System.Reflection.Metadata;

namespace Persistence.DataGenerator;

public class ReviewGenerator
{
    public static Review[] Generate(Product[] products, User[] users)
        => [.. new Faker<Review>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e=>e.Id,f=>f.Random.Uuid())
            .RuleFor(e=>e.CreatedAt,f=>f.Date.Past())
            .RuleFor(e=>e.UpdatedAt,f=>f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.DeletedAt,(f,e) => f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.ProductId,f=>f.PickRandom(products).Id)
            .RuleFor(e=>e.UserId,f=>f.PickRandom(users).Id)
            .RuleFor(e=>e.Rating,f=>f.Random.Number(1,5))
            .RuleFor(e=>e.Content,f=>f.Lorem.Text())
            .Generate(100)
            .ToArray()
            ];
}