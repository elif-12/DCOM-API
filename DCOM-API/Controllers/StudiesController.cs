using DCOM_API.Common;
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
    public async Task<IActionResult> GetStudies([FromQuery] PageRequest request)
    {
        var studies = await _studyService.GetAllAsync(request);
        return Ok(ApiResponse<PageResponse<StudySummary>>.Success(studies));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateStudyRequest request)
    {
        var ok = await _studyService.UpdateAsync(id, request);
        return ok
            ? Ok(ApiResponse<bool>.Success(true, "Güncellendi."))
            : NotFound(ApiResponse<bool>.Fail("Kayıt bulunamadı.", "NOT_FOUND"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _studyService.DeleteAsync(id);
        return ok
            ? Ok(ApiResponse<bool>.Success(true, "Silindi."))
            : NotFound(ApiResponse<bool>.Fail("Kayıt bulunamadı.", "NOT_FOUND"));
    }
}