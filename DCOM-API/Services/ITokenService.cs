using DCOM_API.Entities;

namespace DCOM_API.Services;

public interface ITokenService
{
    string CreateToken(User user);
}
