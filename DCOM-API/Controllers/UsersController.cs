using DCOM_API.Dtos;
using DCOM_API.Entities;
using DCOM_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DCOM_API.Common;

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
    public async Task<IActionResult> GetAll()
    => Ok(ApiResponse<List<UserResponse>>.Success(await _userService.GetAllAsync()));

    [HttpPost]
    public async Task<IActionResult> CreateDoctor(CreateUserRequest request)
    {
        var user = await _userService.CreateDoctorAsync(request);
        return Ok(ApiResponse<UserResponse>.Success(user, "Kullanıcı oluşturuldu."));
    }

   
}