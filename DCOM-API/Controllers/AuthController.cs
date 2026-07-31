using DCOM_API.Application.Interfaces;
using DCOM_API.Common;
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
    private readonly ITokenStore _tokenStore;
    private readonly int _idleMinutes;

    public AuthController(IUserService userService, ITokenService tokenService,
                         ITokenStore tokenStore, IConfiguration config)
    {
        _userService = userService;
        _tokenService = tokenService;
        _tokenStore = tokenStore;
        _idleMinutes = config.GetValue<int>("Jwt:IdleMinutes");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userService.ValidateCredentialsAsync(request.Username, request.Password);
        if (user is null)
            return Unauthorized(ApiResponse<LoginResponse>.Fail("Kullanıcı adı veya şifre hatalı.", "INVALID_CREDENTIALS"));

        var tokenResult = _tokenService.CreateToken(user);
        await _tokenStore.SetAsync(tokenResult.TokenId, TimeSpan.FromMinutes(_idleMinutes));  // idle takibini başlat

        var response = new LoginResponse(tokenResult.Token, user.Username, user.FullName, user.Role.ToString());
        return Ok(ApiResponse<LoginResponse>.Success(response));
    }
}
