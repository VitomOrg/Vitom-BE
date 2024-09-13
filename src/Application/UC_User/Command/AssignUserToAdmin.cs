using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_User.Command;

public class AssignUserToAdmin
{
    public record Command(string UserId) : IRequest<Result>;

    public class Handler(IVitomDbContext context) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            // get updating user
            User? checkingUser = await context.Users
                .SingleOrDefaultAsync(u => u.Id.Equals(request.UserId) && u.DeletedAt == null, cancellationToken);
            if (checkingUser is null) return Result.NotFound("User is not found !");
            checkingUser.AssignToAdmin();
            // save to db
            await context.SaveChangesAsync(cancellationToken);
            // return result
            return Result.NoContent();
        }
    }
}