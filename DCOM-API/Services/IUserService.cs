using DCOM_API.Dtos;
using DCOM_API.Entities;

namespace DCOM_API.Services;

public interface IUserService
{
    Task<User?> ValidateCredentialsAsync(string username, string password);
    Task<UserResponse> CreateDoctorAsync(CreateUserRequest request);
    Task<List<UserResponse>> GetAllAsync();
}
