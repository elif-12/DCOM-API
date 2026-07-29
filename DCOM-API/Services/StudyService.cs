using DCOM_API.Application.Interfaces;

namespace DCOM_API.Services;

public class StudyService : IStudyService
{
    private readonly IStudyRepository _studies;

    public StudyService(IStudyRepository studies)
    {
        _studies = studies;
    }

    public async Task<List<StudySummary>> GetAllAsync()
    {
        var studies = await _studies.GetAllWithDetailsAsync();

        return studies.Select(s => new StudySummary(
            s.Id,
            s.StudyInstanceUid,
            s.Description,
            s.StudyDate,
            s.Patient.PatientName,
            s.Patient.PatientId,
            s.Series.Count,
            s.Series.Sum(se => se.DicomFiles.Count)
        )).ToList();
    }
}