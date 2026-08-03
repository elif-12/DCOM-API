using DCOM_API.Application.Interfaces;
using DCOM_API.Entities;
using DCOM_API.Services;
using Moq;

namespace DCOM_API.Tests;

public class StudyServiceTests
{
    private readonly Mock<IStudyRepository> _studies = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private StudyService CreateSut() => new(_studies.Object, _uow.Object);

    [Fact] // Olmayan id ile güncelleme -> false, kayıt yapılmamalı
    public async Task Update_NotFound_ReturnsFalse()
    {
        _studies.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Study?)null);

        var result = await CreateSut().UpdateAsync(Guid.NewGuid(), new UpdateStudyRequest("aciklama", null));

        Assert.False(result);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact] // Var olan kayıt güncellenmeli, alan değişmeli, kaydedilmeli
    public async Task Update_ExistingRecord_UpdatesAndReturnsTrue()
    {
        var id = Guid.NewGuid();
        var study = new Study { Id = id, StudyInstanceUid = "1.2.3" };
        _studies.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(study);

        var result = await CreateSut().UpdateAsync(id, new UpdateStudyRequest("yeni aciklama", null));

        Assert.True(result);
        Assert.Equal("yeni aciklama", study.Description);
        _studies.Verify(r => r.Update(study), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact] // Olmayan id ile silme -> false, kayıt yapılmamalı
    public async Task Delete_NotFound_ReturnsFalse()
    {
        _studies.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Study?)null);

        var result = await CreateSut().DeleteAsync(Guid.NewGuid());

        Assert.False(result);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact] // Var olan kayıt silinmeli (soft delete) ve kaydedilmeli
    public async Task Delete_ExistingRecord_DeletesAndReturnsTrue()
    {
        var id = Guid.NewGuid();
        var study = new Study { Id = id, StudyInstanceUid = "1.2.3" };
        _studies.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(study);

        var result = await CreateSut().DeleteAsync(id);

        Assert.True(result);
        _studies.Verify(r => r.Delete(study), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
