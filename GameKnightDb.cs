using BoardGameAPI.Models;
using Microsoft.EntityFrameworkCore;

public class GameKnightDb : DbContext
{
    public GameKnightDb(DbContextOptions<GameKnightDb> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<BoardGame> BoardGames => Set<BoardGame>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User configuration
        modelBuilder.Entity<User>()
            .HasKey(u => u.Email);

        // BoardGame configuration
        modelBuilder.Entity<BoardGame>()
            .HasKey(bg => bg.Id);

        // Configure many-to-many relationship between User and BoardGame
        modelBuilder.Entity<User>()
            .HasMany(u => u.BoardGames)
            .WithMany();
    }
}