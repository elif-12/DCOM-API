using DCOM_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DCOM_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudiesController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudiesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetStudies()
    {
        var studies = await _context.Studies
            .Include(s => s.Patient)
            .Include(s => s.Series)
                .ThenInclude(se => se.DicomFiles)
            .Select(s => new
            {
                s.Id,
                s.StudyInstanceUid,
                s.Description,
                s.StudyDate,
                PatientName = s.Patient.PatientName,
                PatientId = s.Patient.PatientId,
                SeriesCount = s.Series.Count,
                DicomCount = s.Series.SelectMany(se => se.DicomFiles).Count()
            })
            .ToListAsync();

        return Ok(studies);
    }
}