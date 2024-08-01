using Domain.DomainEvent;
using MediatR;

namespace Application.UC_User;

public class UserGetsEvent : INotificationHandler<UserGetsDomainEvent>
{
    public async Task Handle(UserGetsDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.Run(() => Console.WriteLine($"Gets at {notification.Date}"), cancellationToken);
    }
}