
using Application.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;
public class VitomDBContext(DbContextOptions<VitomDBContext> options) : DbContext(options), IVitomDbContext
{
    public DbSet<User> Users { get; set; }
}