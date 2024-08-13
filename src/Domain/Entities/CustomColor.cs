using Domain.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class CustomColor : Entity
{
    public Guid ProductId { get; set; }
    [MaxLength(15)]
    public required string Name { get; set; }
    [MaxLength(10)]
    public required string Code { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
}