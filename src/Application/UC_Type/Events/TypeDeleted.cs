using Application.Contracts;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Type.Events;

public class TypeDeleted
{
    public class Event(Guid Id) : BaseEvent
    {
        public Guid Id { get; set; } = Id;
    }

    public class Handler(IVitomDbContext context) : INotificationHandler<Event>
    {
        public async Task Handle(Event notification, CancellationToken cancellationToken)
        {
            await context.ProductTypes
                .Where(t => t.TypeId.Equals(notification.Id))
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}