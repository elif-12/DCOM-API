using DCOM_API.Application.Interfaces;
using DCOM_API.Common;
using DCOM_API.Data;
using DCOM_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DCOM_API.Infrastructure;

public class StudyRepository : IStudyRepository
{
    private readonly AppDbContext _context;

    public StudyRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Study?> GetByStudyInstanceUidAsync(string studyInstanceUid) =>
        _context.Studies.FirstOrDefaultAsync(s => s.StudyInstanceUid == studyInstanceUid);

    public async Task AddAsync(Study study) =>
        await _context.Studies.AddAsync(study);

    public async Task<List<Study>> GetAllWithDetailsAsync() =>
        await _context.Studies
            .Include(s => s.Patient)
            .Include(s => s.Series)
                .ThenInclude(se => se.DicomFiles)
            .ToListAsync();

    public async Task<PageResponse<Study>> GetPagedWithDetailsAsync(PageRequest request)
    {
        var query = _context.Studies
            .Include(s => s.Patient)
            .Include(s => s.Series)
                .ThenInclude(se => se.DicomFiles)
            .OrderByDescending(s => s.CreatedDate);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip(request.Skip)
            .Take(request.Take)
            .ToListAsync();

        return new PageResponse<Study>(items, request.PageNumber, request.PageSize, totalCount);
    }

    public Task<Study?> GetByIdAsync(Guid id) =>
        _context.Studies.FirstOrDefaultAsync(s => s.Id == id);

    public void Update(Study study) =>
        _context.Studies.Update(study);

    public void Delete(Study study) =>
        _context.Studies.Remove(study);
}