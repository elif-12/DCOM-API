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
    public async Task<IActionResult> GetAll()
        => Ok(await _userService.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> CreateDoctor(CreateUserRequest request)
    {
        var user = await _userService.CreateDoctorAsync(request);
        return CreatedAtAction(nameof(GetAll), new { id = user.Id }, user);
    }
}