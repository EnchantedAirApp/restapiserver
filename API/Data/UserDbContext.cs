using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using models.enchantedair.app;

public class UserDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public UserDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<mood> Moods { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("MySQLConnection");

        optionsBuilder.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString),
            options => options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasQueryFilter(u => u.state == RecordState.Active);
        modelBuilder.Entity<mood>().HasQueryFilter(m => m.state == RecordState.Active);
    }
}
