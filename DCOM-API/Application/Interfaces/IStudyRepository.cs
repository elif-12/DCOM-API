using DCOM_API.Entities;

namespace DCOM_API.Application.Interfaces;

public interface IStudyRepository
{
    Task<Study?> GetByStudyInstanceUidAsync(string studyInstanceUid);
    Task<Study?> GetByIdAsync(Guid id);
    Task AddAsync(Study study);
    Task<List<Study>> GetAllWithDetailsAsync();
    void Update(Study study);
    void Delete(Study study);
}