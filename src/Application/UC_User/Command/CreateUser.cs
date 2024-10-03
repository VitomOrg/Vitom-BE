using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.ExternalEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_User.Command;

public class CreateUser
{
    public record Command(UserCreatedClerkEvent AddingEvent) : IRequest<Result>;

    public class Handler(IVitomDbContext context, IMediator mediator) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            ClerkUser clerkUser = request.AddingEvent.data;
            User? checkingUser = await context.Users.FirstOrDefaultAsync(u => u.Id.Equals(clerkUser.id), cancellationToken);
            if (checkingUser is not null) return Result.Success();
            User user = new()
            {
                Id = clerkUser.id,
                Username = clerkUser.username,
                Email = clerkUser.email_addresses.FirstOrDefault()?.email_address ?? string.Empty,
                PhoneNumber = clerkUser.phone_numbers.FirstOrDefault() ?? string.Empty,
                ImageUrl = clerkUser.image_url ?? string.Empty,
                Role = Domain.Enums.RolesEnum.Customer
            };

            await context.Users.AddAsync(user, cancellationToken);
            await mediator.Publish(new UserCreatedClerkEvent(), cancellationToken);
            return Result.Success();
            // return Result.Success();

        }
    }
}