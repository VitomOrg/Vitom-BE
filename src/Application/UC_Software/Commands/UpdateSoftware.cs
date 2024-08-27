using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Software.Commands;

public class UpdateSoftware
{
    public record Command(
        Guid Id,
        string Name,
        string Description
    ) : IRequest<Result>;
    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            // check if current user is admin
            if (!IsCurrentUserAdmin(currentUser.User!)) return Result.Forbidden();
            // get updating software
            Software? updatingSoftware = await context.Softwares.SingleOrDefaultAsync(s => s.Id.Equals(request.Id), cancellationToken);
            if (updatingSoftware is null) return Result.NotFound();
            // update the software
            updatingSoftware.Update(
                name: request.Name,
                description: request.Description
            );
            // save to db
            await context.SaveChangesAsync(cancellationToken);
            // return final result
            return Result.NoContent();
        }
        private static bool IsCurrentUserAdmin(User currentUser)
            => currentUser.Role.Equals(RolesEnum.Admin);
    }
}