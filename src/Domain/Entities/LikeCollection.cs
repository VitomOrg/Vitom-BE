using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class LikeCollection : Entity
{
    public required string UserId { get; set; }
    public required Guid CollectionId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    [ForeignKey(nameof(CollectionId))]
    public Collection Collection { get; set; } = null!;
}