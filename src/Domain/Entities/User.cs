using Domain.Primitives;

namespace Domain.Entities;
public class User : Entity
{
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
}