namespace BoardGameAPI.DTOs.Requests;

public record CreateUserBoardGameRequest(
    string Email,
    BoardGame BoardGame
);