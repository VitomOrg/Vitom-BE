using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Cart : Entity
{
    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    public ICollection<CartItem> CartItems { get; set; } = [];
}