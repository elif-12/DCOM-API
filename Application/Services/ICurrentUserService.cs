namespace DCOM_API.Services;

// O an giriş yapmış kullanıcının Id'sini verir (token'dan okur).
public interface ICurrentUserService
{
    Guid? UserId { get; }
}