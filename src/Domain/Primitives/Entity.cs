using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Primitives;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeletedAt { get; set; }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.UtcNow;
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null!;
    }

    public List<BaseEvent> _domainEvents = [];

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}