using Domain.Primitives;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Type : Entity
{
    [MaxLength(25)]
    public required string Name { get; set; }
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;
    [Range(0, int.MaxValue)]
    public int TotalPurchases { get; set; } = 0;
    public ICollection<ProductType> ProductTypes { get; set; } = [];
}