using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Offices.Models.Settings;

namespace Offices.DataAccess;

public class DbContextBase : DbContext
{
    private readonly DbConnectionSettings _dbSettings;
    public DbContextBase(IOptions<DbConnectionSettings> options)
    {
        _dbSettings = options.Value;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_dbSettings.ConnectionString);
    }
}