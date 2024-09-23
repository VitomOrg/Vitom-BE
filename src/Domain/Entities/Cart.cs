using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Primitives;

namespace Domain.Entities;

public class Cart : Entity
{
    public required string UserId { get; set; }

    [Range(0, long.MaxValue)]
    public long OrderCode { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    public ICollection<CartItem> CartItems { get; set; } = [];
}
