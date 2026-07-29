using DCOM_API.Entities;

namespace DCOM_API.Application.Interfaces;

public interface ISeriesRepository
{
    Task<Series?> GetBySeriesInstanceUidAsync(string seriesInstanceUid);
    Task AddAsync(Series series);
}