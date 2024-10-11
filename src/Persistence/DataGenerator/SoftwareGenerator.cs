using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public class SoftwareGenerator
{
    public static Software[] Generate()
        => [.. new Faker<Software>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e=>e.Id,f=>f.Random.Uuid())
            .RuleFor(e=>e.CreatedAt,f=>f.Date.Past())
            .RuleFor(e=>e.UpdatedAt,f=>f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.DeletedAt,(f,e) => f.Random.Bool() ? f.Date.Past():null!)
            .RuleFor(e=>e.Name,f=>f.Person.FirstName)
            .RuleFor(e=>e.Description,f=>f.Lorem.Word())
            .RuleFor(e=>e.TotalPurchases,f=>f.Random.Number(1,int.MaxValue))
            .Generate(100)
            .ToArray()
            .DistinctBy(e=>e.Name)
            .ToArray()
        ];
}