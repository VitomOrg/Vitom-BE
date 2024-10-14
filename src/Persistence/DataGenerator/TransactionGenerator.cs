using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace Persistence.DataGenerator;

public class TransactionGenerator
{
    public static Transaction[] Generate(User[] users)
        => [.. new Faker<Transaction>()
            .UseSeed(1)
            .UseDateTimeReference(DateTime.UtcNow)
            // base entity
            .RuleFor(e => e.Id, f => f.Random.Uuid())
            .RuleFor(e => e.CreatedAt, f => f.Date.Past())
            .RuleFor(e => e.UpdatedAt, f => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.DeletedAt, (f, e) => f.Random.Bool() ? f.Date.Past() : null!)
            .RuleFor(e => e.UserId, f => f.PickRandom(users).Id)
            .RuleFor(e => e.TotalAmount, f => f.Random.Decimal(1, 9999999999))
            .RuleFor(e => e.PaymentMethod, f => f.PickRandom<PaymentMethodEnum>())
            .RuleFor(e => e.TransactionStatus, f => f.PickRandom<TransactionStatusEnum>())
            .Generate(100)
            .ToArray()
        ];
}