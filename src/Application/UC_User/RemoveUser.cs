using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_User;

public class RemoveUser
{
    public record RemoveUserQuery(Guid UserId) : IRequest<Result>;
    public class RemoveUserHandler(IVitomDbContext context) : IRequestHandler<RemoveUserQuery, Result>
    {
        public async Task<Result> Handle(RemoveUserQuery request, CancellationToken cancellationToken)
        {
            User? user = await context.Users.FirstOrDefaultAsync(u => u.Id.Equals(request.UserId), cancellationToken);
            if (user is null) return Result.NotFound("User is not found");
            context.Users.Remove(user);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}