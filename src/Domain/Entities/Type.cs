using Domain.Primitives;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Type : Entity
{
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int TotalPurchases { get; set; } = 0;
    public ICollection<ProductType> ProductTypes { get; set; } = [];

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}