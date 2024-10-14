using Application.Caches.Events;
using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Software.Commands;

public class DeleteSoftware
{
    public record Command(Guid Id) : IRequest<Result>;
    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            // check if current user is admin
            if (!currentUser.User!.IsAdmin()) return Result.Forbidden();
            // get deleting software
            Software? deletingSoftware = await context.Softwares
                .Where(s => s.DeletedAt == null)
                .SingleOrDefaultAsync(s => s.Id.Equals(request.Id), cancellationToken);
            if (deletingSoftware is null) return Result.NotFound();
            // if deleted at is not null means already deleted
            if (deletingSoftware.DeletedAt is not null) return Result.Error($"Software with id {request.Id} has already been deleted");
            // soft delete software
            deletingSoftware.Delete();
            deletingSoftware.AddDomainEvent(new EntityRemove.Event("software"));
            await context.SaveChangesAsync(cancellationToken);
            // return result
            return Result.NoContent();
        }
    }
}