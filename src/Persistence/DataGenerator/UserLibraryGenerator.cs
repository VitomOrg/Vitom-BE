using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public class UserLibraryGenerator
{
    public static UserLibrary[] Generate(User[] users, Product[] products)
        => [.. new Faker<UserLibrary>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e=>e.Id,f=>f.Random.Uuid())
            .RuleFor(e=>e.CreatedAt,f=>f.Date.Past())
            .RuleFor(e=>e.UpdatedAt,f=>f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.DeletedAt,(f,e) => f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.UserId,f=>f.PickRandom(users).Id)
            .RuleFor(e=>e.ProductId,f=>f.PickRandom(products).Id)
            .Generate(100)
            .ToArray()
            .DistinctBy(e=>new {e.UserId,e.ProductId})
            .ToArray()
        ];
}