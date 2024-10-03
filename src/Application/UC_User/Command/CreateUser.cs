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

    public class Handler(IVitomDbContext context, IMediator mediator) : IRequestHandler<Command, Result>
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
                Role = Domain.Enums.RolesEnum.Customer
            };
            Console.WriteLine("user.Id: " + user.Id);
            Console.WriteLine("user.Email: " + user.Email);
            Console.WriteLine("user.Username: " + user.Username);
            Console.WriteLine("user.Phone: " + user.PhoneNumber);
            Console.WriteLine("user.ImageUrl: " + user.ImageUrl);

            if (user.Id.Equals(string.Empty) || user.Username.Equals(string.Empty) || user.Email.Equals(string.Empty))
                return Result.NotFound("User is not found !");
            await context.Users.AddAsync(user, cancellationToken);
            await mediator.Publish(new UserCreatedEvent.Event(user.Email), cancellationToken);
            return Result.Success();
            // return Result.Success();
        }
    }
}