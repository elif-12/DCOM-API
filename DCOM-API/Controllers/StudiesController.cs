using DCOM_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DCOM_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudiesController : ControllerBase
{
    private readonly IStudyService _studyService;

    public StudiesController(IStudyService studyService)
    {
        _studyService = studyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetStudies()
    {
        var studies = await _studyService.GetAllAsync();
        return Ok(studies);
    }
}