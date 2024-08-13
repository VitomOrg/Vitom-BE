using Domain.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Collection : Entity
{
    public Guid UserId { get; set; }
    [MaxLength(30)]
    public required string Name { get; set; }
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = true;
    public int TotalLiked { get; set; } = 0;
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    public ICollection<CollectionProduct> CollectionProducts { get; set; } = [];
    public ICollection<LikeCollection> LikeCollections { get; set; } = [];
}