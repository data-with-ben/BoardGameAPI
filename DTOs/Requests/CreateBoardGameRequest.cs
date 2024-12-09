namespace BoardGameAPI.DTOs.Requests;

public record CreateBoardGameRequest(
    string Name,
    string Description
);