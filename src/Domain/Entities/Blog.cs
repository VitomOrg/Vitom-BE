using Domain.Primitives;

namespace Domain.Entities;

public class Blog : Entity
{
    public required string UserId { get; set; }
    public required string Title { get; set; }
    public string Content { get; set; } = string.Empty;
}