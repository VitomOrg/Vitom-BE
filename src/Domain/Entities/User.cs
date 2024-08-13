using Domain.Enums;
using Domain.Primitives;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;
public class User : Entity
{
    public required RolesEnum Role { get; set; }
    [MaxLength(15)]
    public required string Username { get; set; }
    [MaxLength(50)]
    public required string Email { get; set; }
    [MaxLength(15)]
    public required string PhoneNumber { get; set; }
    public Cart Cart { get; set; } = null!;
    public ICollection<LikeProduct> LikeProduct { get; set; } = null!;
    public ICollection<LikeCollection> LikeCollection { get; set; } = null!;
    public ICollection<UserLibrary> UserLibrary { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = [];
    public ICollection<Collection> Collections { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
}