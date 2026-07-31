using DCOM_API.Entities;

namespace DCOM_API.Services;

public interface ITokenService
{
    TokenResult CreateToken(User user);
}
public record TokenResult(string Token, string TokenId);
