using DCOM_API.Entities;

namespace DCOM_API.Dtos;

public record LoginRequest(string Username, string Password);

public record LoginResponse(string Token, string Username, string FullName, string Role);

public record CreateUserRequest(string Username, string Password, string FullName);

public record UserResponse(Guid Id, string Username, string FullName, string Role, bool IsActive);
