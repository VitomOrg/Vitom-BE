using Domain.Primitives;
using MediatR;

namespace Persistence.Extensions;

public static class MediatorExtension
{
    public static async Task DispatchDomainEvents(this IMediator mediator, VitomDBContext context, CancellationToken cancellationToken = default)
    {
        var entities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Count != 0);

        var domainEvents = entities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        entities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent, cancellationToken);
    }
}