using Domain.Primitives;

namespace Domain.Entities;

public class BlogImage : Entity
{
    public required Guid BlogId { get; set; }
    public string Url { get; set; } = string.Empty;
}