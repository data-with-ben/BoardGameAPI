namespace BoardGameAPI.DTOs.Responses;

public record BoardGameResponse(
    int Id,
    string Name,
    string Description,
    string Slug
);
