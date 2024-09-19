using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public static class BlogGenerator
{
    public static Blog[] Generate(User[] users)
        => [.. new Faker<Blog>()
            .UseDateTimeReference(DateTime.UtcNow)
            .RuleFor(b => b.Id, f => f.Random.Uuid())
            .RuleFor(b => b.UserId, f => f.PickRandom(users).Id)
            .RuleFor(b => b.Title, f => f.Lorem.Sentence())
            .RuleFor(b => b.Content, f => f.Lorem.Paragraphs(3))
            .RuleFor(b => b.CreatedAt, f => f.Date.Past())
            .RuleFor(b => b.UpdatedAt, f => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(b => b.DeletedAt, f => f.Random.Bool() ? f.Date.Past() : null!)
            .Generate(100)];
}