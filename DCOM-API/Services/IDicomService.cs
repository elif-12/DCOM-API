namespace DCOM_API.Services;

public interface IDicomService
{
    Task<DicomUploadResult> UploadAsync(IFormFile file);
}

public record DicomUploadResult(
    bool Success,
    string? Error,
    string? FileName,
    string? FileUrl,
    string? PatientId,
    string? PatientName,
    string? StudyInstanceUid,
    string? SeriesInstanceUid,
    DateTime? StudyDate,
    string? StudyDescription,
    string? Modality);