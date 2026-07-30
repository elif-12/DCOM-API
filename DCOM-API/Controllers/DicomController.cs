using DCOM_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DCOM_API.Common;

namespace DCOM_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DicomController : ControllerBase
{
    private readonly IDicomService _dicomService;

    public DicomController(IDicomService dicomService)
    {
        _dicomService = dicomService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] DicomUploadRequest request)
    {
        if (request.File is null || request.File.Length == 0)
            return BadRequest(ApiResponse<DicomUploadResult>.Fail("Dosya boş.", "EMPTY_FILE"));

        var result = await _dicomService.UploadAsync(request.File);

        if (!result.Success)
            return BadRequest(ApiResponse<DicomUploadResult>.Fail(result.Error ?? "Yükleme başarısız.", "UPLOAD_FAILED"));

        return Ok(ApiResponse<DicomUploadResult>.Success(result));
    }

    public class DicomUploadRequest
    {
        public IFormFile? File { get; set; }
    }
}