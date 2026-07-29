using DCOM_API.Entities;

namespace DCOM_API.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ExistsByUsernameAsync(string username);
    Task AddAsync(User user);
    Task<List<User>> GetAllAsync();
}