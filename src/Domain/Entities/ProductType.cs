using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ProductType : Entity
{
    public required Guid ProductId { get; set; }
    public required Guid TypeId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
    [ForeignKey(nameof(TypeId))]
    public Type Type { get; set; } = null!;
}