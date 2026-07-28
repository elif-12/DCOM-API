using DCOM_API.Dtos;
using DCOM_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DCOM_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userService.ValidateCredentialsAsync(request.Username, request.Password);
        if (user is null)
            return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı." });

        var token = _tokenService.CreateToken(user);
        return Ok(new LoginResponse(token, user.Username, user.FullName, user.Role.ToString()));
    }
}
