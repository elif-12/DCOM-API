using DCOM_API.Application.Interfaces;
using DCOM_API.Common;
using DCOM_API.Dtos;
using DCOM_API.Entities;

namespace DCOM_API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository users, IUnitOfWork unitOfWork)
    {
        _users = users;
        _unitOfWork = unitOfWork;
    }

    public async Task<User?> ValidateCredentialsAsync(string username, string password)
    {
        var user = await _users.GetByUsernameAsync(username);
        if (user is null || !user.IsActive) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;
        return user;
    }

    public async Task<UserResponse> CreateDoctorAsync(CreateUserRequest request)
    {
        if (await _users.ExistsByUsernameAsync(request.Username))
            throw new InvalidOperationException("Bu kullanıcı adı zaten kullanılıyor.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FullName = request.FullName,
            Role = UserRole.Doctor,
            IsActive = true
        };

        await _users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new UserResponse(user.Id, user.Username, user.FullName, user.Role.ToString(), user.IsActive);
    }

    public async Task<PageResponse<UserResponse>> GetAllAsync(PageRequest request)
    {
        var paged = await _users.GetPagedAsync(request);

        var items = paged.Items
            .Select(u => new UserResponse(u.Id, u.Username, u.FullName, u.Role.ToString(), u.IsActive))
            .ToList();

        return new PageResponse<UserResponse>(items, paged.PageNumber, paged.PageSize, paged.TotalCount);
    }
}