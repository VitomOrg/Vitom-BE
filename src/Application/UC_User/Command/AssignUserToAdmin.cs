using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_User.Command;

public class AssignUserToAdmin
{
    public record Command() : IRequest<Result>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            // get updating user
            User? checkingUser = await context.Users
                .SingleOrDefaultAsync(u => u.Id.Equals(currentUser.User!.Id) && u.DeletedAt == null, cancellationToken);
            if (checkingUser is null) return Result.NotFound("User is not found !");
            checkingUser.AssignToAdmin();
            // save to db
            await context.SaveChangesAsync(cancellationToken);
            // return result
            return Result.NoContent();
        }
    }
}