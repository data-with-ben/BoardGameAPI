namespace BoardGameAPI.Models;

public class User {
    public required string Handle { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public virtual List<BoardGame> BoardGames { get; set; } = new List<BoardGame>();
}