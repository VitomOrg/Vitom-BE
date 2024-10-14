using Domain.Enums;
using Domain.Primitives;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User : Entity
{
    public new string Id { get; set; } = string.Empty;
    public required RolesEnum Role { get; set; }
    public bool IsLicense { get; set; } = false;

    public required string Username { get; set; }
    public required string Email { get; set; }
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