using Application.Caches.Events;
using Application.Contracts;
using Application.UC_Type.Events;
using Ardalis.Result;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Type = Domain.Entities.Type;

namespace Application.UC_Type.Commands;

public class DeleteType
{
    public record Command(Guid Id) : IRequest<Result>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!currentUser.User!.IsAdmin())
                return Result.Forbidden();

            Type? deletingType = await context.Types
                .Where(t => t.DeletedAt == null)
                .SingleOrDefaultAsync(
                t => t.Id.Equals(request.Id),
                cancellationToken
            );

            if (deletingType is null)
                return Result.NotFound();

            if (deletingType.DeletedAt is not null)
                return Result.Error($"Type with id {request.Id} has already been deleted");

            deletingType.Delete();
            deletingType.AddDomainEvent(new EntityRemove.Event("type"));
            deletingType.AddDomainEvent(new TypeDeleted.Event(deletingType.Id));
            await context.SaveChangesAsync(cancellationToken);

            return Result.NoContent();
        }
    }
}