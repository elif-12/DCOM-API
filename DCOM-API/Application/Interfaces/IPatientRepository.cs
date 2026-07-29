using DCOM_API.Entities;

namespace DCOM_API.Application.Interfaces;

public interface IPatientRepository
{
    Task<Patient?> GetByPatientIdAsync(string patientId);
    Task AddAsync(Patient patient);
}