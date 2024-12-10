namespace BoardGameAPI.DTOs.Requests;

public record CreateUserRequest(
    string Handle,
    string FirstName,
    string LastName,
    string Email
);