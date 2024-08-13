using Domain.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Review : Entity
{
    public required Guid ProductId { get; set; }
    public required Guid UserId { get; set; }
    [Range(1,5)]
    public required int Rating { get; set; }
    public required string Content { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}