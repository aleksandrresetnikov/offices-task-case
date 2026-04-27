using System.Reflection;
using Microsoft.EntityFrameworkCore;

using Offices.Models.Entities;

namespace Offices.DataAccess;

public class DellinDictionaryDbContext : DbContext
{
    public DbSet<Office> Offices { get; set; } = null!;
    public DbSet<Phone> Phones { get; set; } = null!;
    
    public DellinDictionaryDbContext(DbContextOptions<DellinDictionaryDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}