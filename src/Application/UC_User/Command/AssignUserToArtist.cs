using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_User.Command;

public class AssignUserToArtist
{
    public record Command(string Id) : IRequest<Result>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            // check if current user is admin
            if (!currentUser.User!.IsAdmin()) return Result.Forbidden();
            // get updating user
            User? checkingUser = await context.Users.SingleOrDefaultAsync(u => u.Id.Equals(request.Id) && u.DeletedAt == null, cancellationToken);
            if (checkingUser is null) return Result.NotFound("User is not found !");
            // updating user
            checkingUser.AssignToArtist();
            // save to db
            await context.SaveChangesAsync(cancellationToken);
            // return result
            return Result.NoContent();
        }
    }
}