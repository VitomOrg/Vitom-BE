using Application.Contracts;
using Domain.Primitives;
using MediatR;

namespace Application.UC_Cart.Events;

public class CartUpdated
{
    public class Event : BaseEvent;

    public class Handler(ICacheServices cacheServices) : INotificationHandler<Event>
    {
        public async Task Handle(Event notification, CancellationToken cancellationToken)
        {
            await cacheServices.RemoveByPrefixAsync("cart", cancellationToken);
        }
    }
}