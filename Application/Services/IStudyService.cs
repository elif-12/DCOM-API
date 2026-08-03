using DCOM_API.Common;

namespace DCOM_API.Services;

public interface IStudyService
{
    Task<PageResponse<StudySummary>> GetAllAsync(PageRequest request);
    Task<bool> UpdateAsync(Guid id, UpdateStudyRequest request);
    Task<bool> DeleteAsync(Guid id);
}

// Güncellenebilir alanlar (kimlik olan StudyInstanceUid değişmez)
public record UpdateStudyRequest(string? Description, DateTime? StudyDate);

public record StudySummary(
    Guid Id,
    string StudyInstanceUid,
    string? Description,
    DateTime? StudyDate,
    string PatientName,
    string PatientId,
    int SeriesCount,
    int DicomCount);