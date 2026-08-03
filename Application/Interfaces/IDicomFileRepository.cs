using DCOM_API.Entities;

namespace DCOM_API.Application.Interfaces;

public interface IDicomFileRepository
{
    Task AddAsync(DicomFile dicomFile);
}