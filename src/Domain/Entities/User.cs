using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Primitives;

namespace Domain.Entities;

public class User : Entity
{
    public new string Id { get; set; } = string.Empty;
    public required RolesEnum Role { get; set; }
    public bool IsLicense { get; set; } = false;

    [MaxLength(25)]
    public required string Username { get; set; }

    [MaxLength(50)]
    public required string Email { get; set; }

    [MaxLength(25)]
    public required string PhoneNumber { get; set; }
    public required string ImageUrl { get; set; } = string.Empty;

    public Cart Cart { get; set; } = null!;
    public ICollection<LikeProduct> LikeProduct { get; set; } = null!;
    public ICollection<LikeCollection> LikeCollection { get; set; } = null!;
    public ICollection<UserLibrary> UserLibrary { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = [];
    public ICollection<Collection> Collections { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<Blog> Blogs { get; set; } = [];

    public bool IsAdmin() => Role.Equals(RolesEnum.Admin);

    public void AssignToArtist()
    {
        Role = RolesEnum.Organization;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void AssignToAdmin()
    {
        Role = RolesEnum.Admin;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public bool IsOrganization() => Role.Equals(RolesEnum.Organization);
}
