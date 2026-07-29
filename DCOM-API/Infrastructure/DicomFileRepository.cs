using DCOM_API.Application.Interfaces;
using DCOM_API.Data;
using DCOM_API.Entities;

namespace DCOM_API.Infrastructure;

public class DicomFileRepository : IDicomFileRepository
{
    private readonly AppDbContext _context;

    public DicomFileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(DicomFile dicomFile) =>
        await _context.DicomFiles.AddAsync(dicomFile);
}