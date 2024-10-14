using Domain.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class CartItem : Entity
{
    public required Guid CartId { get; set; }
    public required Guid ProductId { get; set; }

    [Range(1, 9999999999)]
    [RegularExpression(@"^\d+(\.\d{1,2})?$")]
    public decimal PriceAtPurchase { get; set; } = 0;

    [ForeignKey(nameof(CartId))]
    public Cart Cart { get; set; } = null!;

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
}