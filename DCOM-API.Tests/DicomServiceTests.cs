using DCOM_API.Application.Interfaces;
using DCOM_API.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;

namespace DCOM_API.Tests;

public class DicomServiceTests
{
    private static DicomService CreateSut() => new(
        new Mock<IPatientRepository>().Object,
        new Mock<IStudyRepository>().Object,
        new Mock<ISeriesRepository>().Object,
        new Mock<IDicomFileRepository>().Object,
        new Mock<IUnitOfWork>().Object,
        new Mock<IWebHostEnvironment>().Object);

    [Fact] // Boş dosya yüklenince başarısız (Success = false) dönmeli
    public async Task Upload_EmptyFile_ReturnsFailure()
    {
        var file = new Mock<IFormFile>();
        file.Setup(f => f.Length).Returns(0);

        var result = await CreateSut().UploadAsync(file.Object);

        Assert.False(result.Success);
        Assert.Equal("Dosya boş.", result.Error);
    }
}
