using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public class LikeCollectionGenerator
{
    public static LikeCollection[] Generate(Collection[] collections, User[] users)
        => [.. new Faker<LikeCollection>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e => e.Id, f => f.Random.Uuid())
            .RuleFor(e => e.CreatedAt, f => f.Date.Past())
            .RuleFor(e => e.UpdatedAt, f => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.DeletedAt, (f, e) => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.UserId, f => f.PickRandom(users).Id)
            .RuleFor(e => e.CollectionId, f => f.PickRandom(collections).Id)
            .Generate(100)
            .ToArray()
            .DistinctBy(e => new { e.UserId, e.CollectionId })
            .ToArray()
            ];
}