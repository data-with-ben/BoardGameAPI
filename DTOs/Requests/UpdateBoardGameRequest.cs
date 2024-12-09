namespace BoardGameAPI.DTOs.Requests;

public record UpdateBoardGameRequest(
    string Name,
    string Description
);