using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class BlogImage : Entity
{
    public required Guid BlogId { get; set; }
    public string Url { get; set; } = string.Empty;
    [ForeignKey(nameof(BlogId))]
    public Blog Blogs { get; set; } = null!;
}