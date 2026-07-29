using DCOM_API.Application.Interfaces;
using DCOM_API.Data;
using DCOM_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DCOM_API.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByUsernameAsync(string username) =>
        _context.Users.FirstOrDefaultAsync(u => u.Username == username);

    public Task<bool> ExistsByUsernameAsync(string username) =>
        _context.Users.AnyAsync(u => u.Username == username);

    public async Task AddAsync(User user) =>
        await _context.Users.AddAsync(user);

    public async Task<List<User>> GetAllAsync() =>
        await _context.Users.ToListAsync();
}