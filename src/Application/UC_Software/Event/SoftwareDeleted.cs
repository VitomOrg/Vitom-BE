using Application.Contracts;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Software.Event;

public class SoftwareDeleted
{
    public class Event(Guid Id) : BaseEvent
    {
        public Guid Id { get; set; } = Id;
    }

    public class Handler(IVitomDbContext context) : INotificationHandler<Event>
    {
        public async Task Handle(Event notification, CancellationToken cancellationToken)
        {
            await context.ProductSoftwares
                .Where(t => t.SoftwareId.Equals(notification.Id))
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}