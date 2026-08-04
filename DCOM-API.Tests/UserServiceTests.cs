using DCOM_API.Application.Interfaces;
using DCOM_API.Dtos;
using DCOM_API.Entities;
using DCOM_API.Services;
using Moq;

namespace DCOM_API.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private UserService CreateSut() => new(_users.Object, _uow.Object);

    [Fact] // Yanlış şifre girilince null dönmeli (giriş reddedilmeli)
    public async Task ValidateCredentials_WrongPassword_ReturnsNull()
    {
        var user = new User
        {
            Username = "ali",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("dogruSifre"),
            IsActive = true
        };
        _users.Setup(r => r.GetByUsernameAsync("ali")).ReturnsAsync(user);

        var result = await CreateSut().ValidateCredentialsAsync("ali", "yanlisSifre");

        Assert.Null(result);
    }

    [Fact] // Doğru şifre girilince kullanıcı dönmeli
    public async Task ValidateCredentials_CorrectPassword_ReturnsUser()
    {
        var user = new User
        {
            Username = "ali",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("dogruSifre"),
            IsActive = true
        };
        _users.Setup(r => r.GetByUsernameAsync("ali")).ReturnsAsync(user);

        var result = await CreateSut().ValidateCredentialsAsync("ali", "dogruSifre");

        Assert.NotNull(result);
        Assert.Equal("ali", result!.Username);
    }

    [Fact] // Kullanıcı bulunamazsa null dönmeli
    public async Task ValidateCredentials_UserNotFound_ReturnsNull()
    {
        _users.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var result = await CreateSut().ValidateCredentialsAsync("yok", "sifre");

        Assert.Null(result);
    }

    [Fact] // Aynı kullanıcı adı varsa exception fırlatmalı
    public async Task CreateDoctor_DuplicateUsername_Throws()
    {
        _users.Setup(r => r.ExistsByUsernameAsync("ali")).ReturnsAsync(true);

        var request = new CreateUserRequest("ali", "sifre", "Ali Veli");

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => CreateSut().CreateDoctorAsync(request));
    }

    [Fact] // Geçerli istekte Doctor rolüyle kullanıcı oluşturup kaydetmeli
    public async Task CreateDoctor_ValidRequest_CreatesDoctorAndSaves()
    {
        _users.Setup(r => r.ExistsByUsernameAsync(It.IsAny<string>())).ReturnsAsync(false);

        var request = new CreateUserRequest("yeni", "sifre123", "Yeni Doktor");

        var result = await CreateSut().CreateDoctorAsync(request);

        Assert.Equal("yeni", result.Username);
        Assert.Equal("Doctor", result.Role);
        _users.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
