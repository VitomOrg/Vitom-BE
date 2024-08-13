using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ProductSoftware : Entity
{
    public required Guid ProductId { get; set; }
    public required Guid SoftwareId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
    [ForeignKey(nameof(SoftwareId))]
    public Software Software { get; set; } = null!;
}