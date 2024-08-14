using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public class CollectionGenerator
{
    public static Collection[] Generate(User[] users)
        => [.. new Faker<Collection>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e=>e.Id,f=>f.Random.Uuid())
            .RuleFor(e=>e.CreatedAt,f=>f.Date.Past())
            .RuleFor(e=>e.UpdatedAt,f=>f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.DeletedAt,(f,e) => f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=> e.UserId,f=>f.PickRandom(users).Id)
            .RuleFor(e=> e.Name,f=>f.Person.FirstName)
            .RuleFor(e=>e.Description,f=>f.Lorem.Word())
            .RuleFor(e=>e.IsPublic,f=>f.Random.Bool())
            .RuleFor(e=>e.TotalLiked,f=>f.PickRandom(1,int.MaxValue))
            .Generate(100)
            .ToArray()
            .DistinctBy(e=>new {e.UserId,e.Name})
            .ToArray()
        ];
}