using Domain.Entities;

namespace Domain.Primitives;

public class CurrentUser
{
    public User? User { get; set; } = null!;
}