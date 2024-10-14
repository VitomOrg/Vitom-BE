using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace Persistence.DataGenerator;

public class ProductGenerator
{
    public static Product[] Generate(User[] users)
        => [.. new Faker<Product>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e => e.Id, f => f.Random.Uuid())
            .RuleFor(e => e.CreatedAt, f => f.Date.Past())
            .RuleFor(e => e.UpdatedAt, f => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.DeletedAt, (f, e) => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.UserId, f => f.PickRandom(users).Id)
            .RuleFor(e => e.License, f => f.PickRandom<LicenseEnum>())
            .RuleFor(e => e.Name, f => f.Vehicle.Model())
            .RuleFor(e => e.Description, f => f.Lorem.Word())
            .RuleFor(e => e.Price, f => f.Random.Decimal(1, 9999999999))
            .RuleFor(e => e.DownloadUrl, f => f.Image.PlaceImgUrl())
            .RuleFor(e => e.TotalPurchases, f => f.Random.Number(1, int.MaxValue))
            .RuleFor(e => e.TotalLiked, f => f.Random.Number(1, int.MaxValue))
            .Generate(100)
            .ToArray()
            .DistinctBy(e => new { e.Name, e.UserId })
            .ToArray()
        ];
}