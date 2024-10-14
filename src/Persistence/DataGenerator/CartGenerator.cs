using Bogus;
using Domain.Entities;

namespace Persistence.DataGenerator;

public class CartGenerator
{
    public static Cart[] Generate(User[] users) =>
        [
            .. new Faker<Cart>()
                .UseSeed(1)
                .UseDateTimeReference(DateTime.UtcNow)
                // base entity
                .RuleFor(e => e.Id, f => f.Random.Uuid())
                .RuleFor(e => e.CreatedAt, f => f.Date.Past())
                .RuleFor(e => e.UpdatedAt, f => f.Random.Bool() ? f.Date.Past() : null!)
                .RuleFor(e => e.DeletedAt, (f, e) => f.Random.Bool() ? f.Date.Past() : null!)
                .RuleFor(e => e.UserId, f => f.PickRandom(users).Id)
                .RuleFor(
                    e => e.OrderCode,
                    f =>
                    {
                        int orderCode;
                        do
                        {
                            orderCode = BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0);
                        } while (orderCode <= 0);

                        return orderCode;
                    }
                )
                .Generate(100)
                .ToArray()
                .DistinctBy(e => e.UserId)
                .ToArray()
        ];
}