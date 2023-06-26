using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

public class SnappFoodDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }

    public SnappFoodDbContext(DbContextOptions<SnappFoodDbContext> options)
        : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var entryEntities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        foreach (var entryEntity in entryEntities)
        {
            if (entryEntity.State != EntityState.Added) continue;
            if (entryEntity.Entity.GetType().GetProperty("CreatedAt") != null)
            {
                entryEntity.Entity.GetType().GetProperty("CreatedAt").SetValue(entryEntity.Entity, DateTime.Now);
            }
        }
        
        return await base.SaveChangesAsync(cancellationToken);
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("SqlConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Assign");
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Title)
            .IsUnique();
    }
    // public class MyDbContextFactory : IDesignTimeDbContextFactory<DbContext>
    // {
    //     public DbContext CreateDbContext(string[] args)
    //     {
    //         var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
    //         var configuration = new ConfigurationBuilder()
    //             .SetBasePath(Directory.GetCurrentDirectory())
    //             .AddJsonFile("appsettings.json")
    //             .Build();
    //         var connectionString = configuration.GetConnectionString("SqlConnection");
    //         optionsBuilder.UseSqlServer(connectionString);
    //
    //         return new DbContext(optionsBuilder.Options);
    //     }
    // }
}