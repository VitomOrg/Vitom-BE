using Domain.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class TransactionDetail : Entity
{
    public required Guid TransactionId { get; set; }
    public required Guid ProductId { get; set; }
    [Range(0, 9999999999)]
    [RegularExpression(@"^\d+(\.\d{1,2})?$")]
    public decimal PriceAtPurchase { get; set; } = 0;
    [ForeignKey(nameof(TransactionId))]
    public Transaction Transaction { get; set; } = null!;
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
}