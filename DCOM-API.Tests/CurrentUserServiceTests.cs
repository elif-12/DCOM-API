using System.Security.Claims;
using DCOM_API.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace DCOM_API.Tests;

public class CurrentUserServiceTests
{
    [Fact] // Token'da NameIdentifier claim'i varsa doğru UserId dönmeli
    public void UserId_WhenClaimExists_ReturnsUserId()
    {
        var userId = Guid.NewGuid();
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }))
        };
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var service = new CurrentUserService(accessor.Object);

        Assert.Equal(userId, service.UserId);
    }

    [Fact] // Claim yoksa null dönmeli
    public void UserId_WhenNoClaim_ReturnsNull()
    {
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(new DefaultHttpContext());

        var service = new CurrentUserService(accessor.Object);

        Assert.Null(service.UserId);
    }
}
