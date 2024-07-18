using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Contracts;
public interface IVitomDbContext
{
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}