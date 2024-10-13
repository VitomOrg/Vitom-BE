using Application.Caches.Events;
using Application.Contracts;
using Ardalis.Result;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Type = Domain.Entities.Type;

namespace Application.UC_Type.Commands;

public class UpdateType
{
    public record Command(Guid Id, string Name, string Description) : IRequest<Result>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!currentUser.User!.IsAdmin())
                return Result.Forbidden();

            Type? updatingType = await context.Types.SingleOrDefaultAsync(
                t => t.Id.Equals(request.Id) && t.DeletedAt == null,
                cancellationToken
            );

            if (updatingType is null)
                return Result.NotFound();

            updatingType.Update(name: request.Name, description: request.Description);
            updatingType.AddDomainEvent(new EntityUpdated.Event("type"));
            await context.SaveChangesAsync(cancellationToken);

            return Result.NoContent();
        }
    }
}
