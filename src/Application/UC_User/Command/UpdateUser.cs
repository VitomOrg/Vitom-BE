using Application.Contracts;
using Ardalis.Result;
using Domain.ExternalEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_User.Command;

public class UpdateUser
{
    public record Command(Event AddingEvent) : IRequest<Result>;

    public class Handler(IVitomDbContext context) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            ClerkUser clerkUser = request.AddingEvent.Data!;
            int result = await context.Users
                .Where(u => u.Id.Equals(clerkUser.Id))
                .ExecuteUpdateAsync(e =>
                    e.SetProperty(u => u.Username, clerkUser.Username)
                        .SetProperty(u => u.Email, clerkUser.EmailAddresses.FirstOrDefault()!.EmailAddress)
                        .SetProperty(u => u.PhoneNumber, clerkUser.PhoneNumbers.FirstOrDefault()!.PhoneNumber)
                        .SetProperty(u => u.ImageUrl, clerkUser.ImageUrl), cancellationToken);
            if (result == 0) return Result.NotFound("User is not found !");
            return Result.NoContent();
        }
    }
}