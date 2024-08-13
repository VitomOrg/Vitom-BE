using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class LikeProduct : Entity
{
    public required Guid UserId { get; set; }
    public required Guid ProductId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
}