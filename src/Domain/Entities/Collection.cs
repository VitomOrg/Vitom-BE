using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Collection : Entity
{
    public required string UserId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = true;
    public int TotalLiked { get; set; } = 0;
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    public ICollection<CollectionProduct> CollectionProducts { get; set; } = [];
    public ICollection<LikeCollection> LikeCollections { get; set; } = [];

    public void Update(string name, string description, bool isPublic)
    {
        Name = name;
        Description = description;
        IsPublic = isPublic;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}