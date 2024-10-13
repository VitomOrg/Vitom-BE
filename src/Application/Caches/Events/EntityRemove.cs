using Application.Contracts;
using Domain.Primitives;
using MediatR;

namespace Application.Caches.Events;

public class EntityRemove
{
    public class Event(string cacheName) : BaseEvent
    {
        public string CacheName { get; set; } = cacheName;
    }

    public class Handler(ICacheServices cacheServices) : INotificationHandler<Event>
    {
        public async Task Handle(Event notification, CancellationToken cancellationToken)
        {
            await cacheServices.RemoveByPrefixAsync(notification.CacheName, cancellationToken);
        }
    }
}