using app.enchantedair.model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using models.enchantedair.app;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
namespace app.enchantedair.persistence
{
    public class EnchantedAirDB : DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal user;
        public DbSet<Mood> Moods { get; set; }
        public DbSet<User> Users { get; set; }


        public EnchantedAirDB(DbContextOptions<EnchantedAirDB> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            user = httpContextAccessor?.HttpContext?.User;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mood>().ToTable("moods");
            modelBuilder.Entity<User>().ToTable("users");
        }
    }
}