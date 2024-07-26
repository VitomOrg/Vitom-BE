using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public class UserGenerator
{
    public static User[] Generate()
        => [.. new Faker<User>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            .RuleFor(u=>u.Id,f=>f.Random.Uuid())
            .RuleFor(u=>u.CreatedAt,f=>f.Date.Past())
            .RuleFor(u=>u.UpdatedAt,f=>f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(u=>u.IsDeleted,f=>f.Random.Bool())
            .RuleFor(u=>u.DeletedAt,(f,u) => u.IsDeleted? f.Date.Past():null!)
            .RuleFor(u=>u.Name,f=>f.Person.FullName)
            .RuleFor(u=>u.PhoneNumber,f=>f.Person.Phone)
            .Generate(100)];
}