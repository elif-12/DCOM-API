using DCOM_API.Application.Interfaces;
using DCOM_API.Common;

namespace DCOM_API.Services;

public class StudyService : IStudyService
{
    private readonly IStudyRepository _studies;
    private readonly IUnitOfWork _unitOfWork;

    public StudyService(IStudyRepository studies, IUnitOfWork unitOfWork)
    {
        _studies = studies;
        _unitOfWork = unitOfWork;
    }

    public async Task<PageResponse<StudySummary>> GetAllAsync(PageRequest request)
    {
        var paged = await _studies.GetPagedWithDetailsAsync(request);

        var items = paged.Items.Select(s => new StudySummary(
            s.Id,
            s.StudyInstanceUid,
            s.Description,
            s.StudyDate,
            s.Patient.PatientName,
            s.Patient.PatientId,
            s.Series.Count,
            s.Series.Sum(se => se.DicomFiles.Count)
        )).ToList();

        return new PageResponse<StudySummary>(items, paged.PageNumber, paged.PageSize, paged.TotalCount);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateStudyRequest request)
    {
        var study = await _studies.GetByIdAsync(id);
        if (study is null) return false;

        study.Description = request.Description;
        study.StudyDate = request.StudyDate;

        _studies.Update(study);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var study = await _studies.GetByIdAsync(id);
        if (study is null) return false;

        _studies.Delete(study);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}