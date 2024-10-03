using Application.Contracts;
using MediatR;

namespace Application.UC_User.Events;

public class UserCreatedEvent
{
    public record Event(string Email) : INotification;

    public class Handler(IMailServices mailServices) : INotificationHandler<Event>
    {
        public async Task Handle(Event notification, CancellationToken cancellationToken)
        {
            await mailServices.SendEmailAsync(notification.Email, "Welcome to Vitom", "Thank you for joining us!");
        }
    }
}