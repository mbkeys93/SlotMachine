using Microsoft.EntityFrameworkCore;
using SlotMachineAPI.Models;

namespace SlotMachineAPI.Data;

public class SlotMachineDbContext : DbContext
{
    public SlotMachineDbContext(DbContextOptions<SlotMachineDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Symbol> Symbols { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Balance).HasColumnType("decimal(18,2)");
            entity.HasIndex(e => e.UserName).IsUnique();
            entity.Property(e => e.ModifiedDateTime).HasColumnType("TEXT").HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
        
        // Configure Game entity
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId);
            entity.Property(e => e.BetAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.WinAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.SpinResult).HasMaxLength(50);
            entity.Property(e => e.SpinDateTime).HasColumnType("TEXT").HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Games)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Configure Symbol entity
        modelBuilder.Entity<Symbol>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Value).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Weight).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();
        });
        
        // Seed default symbols
        modelBuilder.Entity<Symbol>().HasData(Symbol.GetDefaultSymbols());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update ModifiedDateTime for all modified User entities
        var modifiedUsers = ChangeTracker.Entries<User>()
            .Where(e => e.State == EntityState.Modified)
            .Select(e => e.Entity);
        
        foreach (var user in modifiedUsers)
        {
            user.ModifiedDateTime = DateTime.UtcNow;
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
} 