using LittleLemon_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace LittleLemon_API.Data;

public class ApplicationDbContext : IdentityDbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Meal> Meals { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Order>()
        .HasMany(e => e.Meals);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var factory = LoggerFactory.Create(builder => builder.AddConsole());
            optionsBuilder.UseLoggerFactory(factory);

            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("AzureDatabaseConnection"));
        }

    }
}
