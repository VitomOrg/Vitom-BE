using Application.Contracts;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Blog.Events;

public class BlogViewed
{
    public class Event(Guid Id) : BaseEvent
    {
        public Guid Id { get; set; } = Id;
    }

    public class Handler(IVitomDbContext context) : INotificationHandler<Event>
    {
        public async Task Handle(Event notification, CancellationToken cancellationToken)
        {
            await context.Blogs
                .Where(b => b.Id == notification.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(b => b.TotalVisit, b => b.TotalVisit + 1),
                    cancellationToken);
        }
    }
}