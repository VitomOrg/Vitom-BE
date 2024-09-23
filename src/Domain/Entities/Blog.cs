using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Blog : Entity
{
    public required string UserId { get; set; }
    public required string Title { get; set; }
    public string Content { get; set; } = string.Empty;
    // Relations
    public ICollection<BlogImage> Images { get; set; } = [];
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public void Update(string title, string content)
    {
        Title = title;
        Content = content;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}