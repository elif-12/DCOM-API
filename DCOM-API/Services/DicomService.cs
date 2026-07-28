using DCOM_API.Data;
using DCOM_API.Entities;
using FellowOakDicom;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using FoDicomFile = FellowOakDicom.DicomFile;

namespace DCOM_API.Services;

public class DicomService : IDicomService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public DicomService(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<DicomUploadResult> UploadAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Fail("Dosya boş.");

        var webRootPath = _environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRootPath))
            webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        var dicomFolder = Path.Combine(webRootPath, "dicoms");
        if (!Directory.Exists(dicomFolder))
            Directory.CreateDirectory(dicomFolder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(dicomFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        FoDicomFile dicom;
        try
        {
            dicom = await FoDicomFile.OpenAsync(filePath);
        }
        catch
        {
            return Fail("Bu dosya geçerli bir DICOM dosyası değil.");
        }

        var patientId = dicom.Dataset.GetSingleValueOrDefault<string>(DicomTag.PatientID, string.Empty);
        var patientName = dicom.Dataset.GetSingleValueOrDefault<string>(DicomTag.PatientName, string.Empty);
        var studyInstanceUid = dicom.Dataset.GetSingleValueOrDefault<string>(DicomTag.StudyInstanceUID, string.Empty);
        var seriesInstanceUid = dicom.Dataset.GetSingleValueOrDefault<string>(DicomTag.SeriesInstanceUID, string.Empty);
        var studyDescription = dicom.Dataset.GetSingleValueOrDefault<string>(DicomTag.StudyDescription, string.Empty);
        var modality = dicom.Dataset.GetSingleValueOrDefault<string>(DicomTag.Modality, string.Empty);
        var studyDateRaw = dicom.Dataset.GetSingleValueOrDefault<string>(DicomTag.StudyDate, string.Empty);

        DateTime? studyDate = null;
        if (DateTime.TryParseExact(
                studyDateRaw,
                "yyyyMMdd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsedDate))
        {
            studyDate = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
        }

        if (string.IsNullOrWhiteSpace(patientId) ||
            string.IsNullOrWhiteSpace(studyInstanceUid) ||
            string.IsNullOrWhiteSpace(seriesInstanceUid))
        {
            return Fail("DICOM içinde gerekli alanlar bulunamadı.");
        }

        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientId == patientId);

        if (patient is null)
        {
            patient = new Patient
            {
                Id = Guid.NewGuid(),
                PatientId = patientId,
                PatientName = string.IsNullOrWhiteSpace(patientName) ? "Unknown" : patientName
            };

            _context.Patients.Add(patient);
        }
        else if (string.IsNullOrWhiteSpace(patient.PatientName) && !string.IsNullOrWhiteSpace(patientName))
        {
            patient.PatientName = patientName;
        }

        var study = await _context.Studies.FirstOrDefaultAsync(s => s.StudyInstanceUid == studyInstanceUid);

        if (study is null)
        {
            study = new Study
            {
                Id = Guid.NewGuid(),
                StudyInstanceUid = studyInstanceUid,
                StudyDate = studyDate,
                Description = studyDescription,
                PatientId = patient.Id
            };

            _context.Studies.Add(study);
        }
        else
        {
            study.PatientId = patient.Id;

            if (!study.StudyDate.HasValue && studyDate.HasValue)
                study.StudyDate = studyDate;

            if (string.IsNullOrWhiteSpace(study.Description) && !string.IsNullOrWhiteSpace(studyDescription))
                study.Description = studyDescription;
        }

        var series = await _context.Series.FirstOrDefaultAsync(s => s.SeriesInstanceUid == seriesInstanceUid);

        if (series is null)
        {
            series = new Series
            {
                Id = Guid.NewGuid(),
                SeriesInstanceUid = seriesInstanceUid,
                Modality = modality,
                StudyId = study.Id
            };

            _context.Series.Add(series);
        }
        else
        {
            series.StudyId = study.Id;

            if (string.IsNullOrWhiteSpace(series.Modality) && !string.IsNullOrWhiteSpace(modality))
                series.Modality = modality;
        }

        var dicomFile = new DCOM_API.Entities.DicomFile
        {
            Id = Guid.NewGuid(),
            FileName = fileName,
            FileUrl = $"/dicoms/{fileName}",
            UploadDate = DateTime.UtcNow,
            SeriesId = series.Id
        };

        _context.DicomFiles.Add(dicomFile);
        await _context.SaveChangesAsync();

        return new DicomUploadResult(
            true, null, fileName, $"/dicoms/{fileName}",
            patientId, patientName, studyInstanceUid, seriesInstanceUid,
            studyDate, studyDescription, modality);
    }

    private static DicomUploadResult Fail(string error) =>
        new(false, error, null, null, null, null, null, null, null, null, null);
}