using Application.Contracts;
using Domain.Primitives;
using MediatR;

namespace Application.UC_User.Events;

public class UserCreatedEvent
{
    public class Event(string Email) : BaseEvent
    {
        public string Email { get; set; } = Email;
    }

    public class Handler(IMailServices mailServices) : INotificationHandler<Event>
    {
        public async Task Handle(Event notification, CancellationToken cancellationToken)
        {
            await mailServices.SendEmailAsync(notification.Email, "Welcome to Vitom", "Thank you for joining us!");
        }
    }
}