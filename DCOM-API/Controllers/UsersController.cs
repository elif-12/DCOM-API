using DCOM_API.Common;
using DCOM_API.Dtos;
using DCOM_API.Entities;
using DCOM_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DCOM_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = nameof(UserRole.SuperAdmin))]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PageRequest request)
        => Ok(ApiResponse<PageResponse<UserResponse>>.Success(await _userService.GetAllAsync(request)));

    [HttpPost]
    public async Task<IActionResult> CreateDoctor(CreateUserRequest request)
    {
        var user = await _userService.CreateDoctorAsync(request);
        return Ok(ApiResponse<UserResponse>.Success(user, "Kullanıcı oluşturuldu."));
    }
}