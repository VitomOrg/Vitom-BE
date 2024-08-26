using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace Persistence.DataGenerator;

public class UserGenerator
{
    public static User[] Generate()
        => [.. new Faker<User>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e=>e.Id,f=>f.Random.Uuid().ToString())
            .RuleFor(e=>e.CreatedAt,f=>f.Date.Past())
            .RuleFor(e=>e.UpdatedAt,f=>f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.DeletedAt,(f,e) => f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.Role,f=>f.PickRandom<RolesEnum>())
            .RuleFor(e=>e.Username,f=>f.Person.FirstName)
            .RuleFor(e=>e.Email,f=>f.Person.Email)
            .RuleFor(e=>e.PhoneNumber,f=>f.Person.Phone)
            .Generate(100)
            .DistinctBy(e=>e.Email)
            .ToArray()
            ];
}