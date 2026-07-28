using DCOM_API.Data;
using Microsoft.EntityFrameworkCore;

namespace DCOM_API.Services;

public class StudyService : IStudyService
{
    private readonly AppDbContext _context;

    public StudyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StudySummary>> GetAllAsync()
    {
        return await _context.Studies
            .Include(s => s.Patient)
            .Include(s => s.Series)
                .ThenInclude(se => se.DicomFiles)
            .Select(s => new StudySummary(
                s.Id,
                s.StudyInstanceUid,
                s.Description,
                s.StudyDate,
                s.Patient.PatientName,
                s.Patient.PatientId,
                s.Series.Count,
                s.Series.SelectMany(se => se.DicomFiles).Count()))
            .ToListAsync();
    }
}