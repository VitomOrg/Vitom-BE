using Application.Caches.Events;
using Application.Contracts;
using Application.UC_User.Events;
using Ardalis.Result;
using Domain.Entities;
using Domain.ExternalEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_User.Command;

public class CreateUser
{
    public record Command(Event AddingEvent) : IRequest<Result>;

    public class Handler(IVitomDbContext context) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            ClerkUser clerkUser = request.AddingEvent.Data!;
            User? checkingUser = await context.Users.FirstOrDefaultAsync(u => u.Id.Equals(clerkUser.Id), cancellationToken);
            if (checkingUser is not null) return Result.Success();
            User user = new()
            {
                Id = clerkUser.Id ?? string.Empty,
                Username = clerkUser.Username ?? string.Empty,
                Email = clerkUser.EmailAddresses.FirstOrDefault()?.EmailAddress ?? string.Empty,
                PhoneNumber = clerkUser.PhoneNumbers.FirstOrDefault()?.PhoneNumber ?? string.Empty,
                ImageUrl = clerkUser.ImageUrl ?? string.Empty,
                Cart = new()
                {
                    UserId = clerkUser.Id ?? string.Empty,
                },
                Role = Domain.Enums.RolesEnum.Customer
            };
            if (user.Id.Equals(string.Empty))
                return Result.NotFound("User is not found !");
            await context.Users.AddAsync(user, cancellationToken);
            user.AddDomainEvent(new EntityCreated.Event("user"));
            user.AddDomainEvent(new UserCreatedEvent.Event(user.Email));
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
            // return Result.Success();
        }
    }
}