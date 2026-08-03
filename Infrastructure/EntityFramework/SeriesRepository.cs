using DCOM_API.Application.Interfaces;
using DCOM_API.Data;
using DCOM_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DCOM_API.Infrastructure;

public class SeriesRepository : ISeriesRepository
{
    private readonly AppDbContext _context;

    public SeriesRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Series?> GetBySeriesInstanceUidAsync(string seriesInstanceUid) =>
        _context.Series.FirstOrDefaultAsync(s => s.SeriesInstanceUid == seriesInstanceUid);

    public async Task AddAsync(Series series) =>
        await _context.Series.AddAsync(series);
}