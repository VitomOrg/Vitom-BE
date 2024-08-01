using Domain.Primitives;

namespace Domain.DomainEvent;

public class UserGetsDomainEvent(DateTimeOffset date) : BaseEvent
{
    public DateTimeOffset Date { get; set; } = date;
}