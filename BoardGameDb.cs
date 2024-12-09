using Microsoft.EntityFrameworkCore;

class BoardGameDb : DbContext
{
    public BoardGameDb(DbContextOptions<BoardGameDb> options)
        : base(options) { }

    public DbSet<BoardGame> BoardGames => Set<BoardGame>();
}