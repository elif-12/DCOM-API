namespace DCOM_API.Services;

public interface IStudyService
{
    Task<List<StudySummary>> GetAllAsync();
}

public record StudySummary(
    Guid Id,
    string StudyInstanceUid,
    string? Description,
    DateTime? StudyDate,
    string PatientName,
    string PatientId,
    int SeriesCount,
    int DicomCount);