using DCOM_API.Application.Interfaces;
using DCOM_API.Data;
using DCOM_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DCOM_API.Infrastructure;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Patient?> GetByPatientIdAsync(string patientId) =>
        _context.Patients.FirstOrDefaultAsync(p => p.PatientId == patientId);

    public async Task AddAsync(Patient patient) =>
        await _context.Patients.AddAsync(patient);
}