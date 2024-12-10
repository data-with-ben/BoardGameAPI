namespace BoardGameAPI.DTOs.Requests;

public record UpdateUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string Handle
);
