using DCOM_API.Common;
using DCOM_API.Dtos;
using DCOM_API.Services;
using Microsoft.AspNetCore.Mvc;
using DCOM_API.Common;

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
            return Unauthorized(ApiResponse<LoginResponse>.Fail("Kullanıcı adı veya şifre hatalı.", "INVALID_CREDENTIALS"));

        var token = _tokenService.CreateToken(user);
        var response = new LoginResponse(token, user.Username, user.FullName, user.Role.ToString());
        return Ok(ApiResponse<LoginResponse>.Success(response));
    }
}
