using Domain.Primitives;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Software : Entity
{
    [MaxLength(15)]
    public required string Name { get; set; }
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;
    [Range(0, int.MaxValue)]
    public int TotalPurchases { get; set; } = 0;
    public ICollection<ProductSoftware> ProductSoftwares { get; set; } = [];
}