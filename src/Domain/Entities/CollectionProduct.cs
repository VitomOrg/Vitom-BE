using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class CollectionProduct : Entity
{
    public required Guid CollectionId { get; set; }
    public required Guid ProductId { get; set; }
    [ForeignKey(nameof(CollectionId))]
    public Collection Collection { get; set; } = null!;
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
}