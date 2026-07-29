using DCOM_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return BadRequest("Dosya boş.");

        var result = await _dicomService.UploadAsync(request.File);

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result);
    }

    public class DicomUploadRequest
    {
        public IFormFile? File { get; set; }
    }
}