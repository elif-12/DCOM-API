using System.IdentityModel.Tokens.Jwt;
using DCOM_API.Entities;
using DCOM_API.Services;
using Microsoft.Extensions.Configuration;

namespace DCOM_API.Tests;

public class TokenServiceTests
{
    private static TokenService CreateSut()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "EliLJJUvD3CSzbesqGjcPlAF8ECEwTiW", // en az 32 karakter
                ["Jwt:Issuer"] = "DCOM-API",
                ["Jwt:Audience"] = "DCOM-API",
                ["Jwt:ExpiryMinutes"] = "60"
            })
            .Build();

        return new TokenService(config);
    }

    [Fact] // Token ve token kimliği (jti) üretilmeli
    public void CreateToken_ReturnsTokenAndId()
    {
        var user = new User { Id = Guid.NewGuid(), Username = "admin", Role = UserRole.SuperAdmin };

        var result = CreateSut().CreateToken(user);

        Assert.False(string.IsNullOrWhiteSpace(result.Token));
        Assert.False(string.IsNullOrWhiteSpace(result.TokenId));
    }

    [Fact] // Üretilen token'ın içindeki jti, dönen TokenId ile aynı olmalı
    public void CreateToken_TokenContainsMatchingJti()
    {
        var user = new User { Id = Guid.NewGuid(), Username = "admin", Role = UserRole.SuperAdmin };

        var result = CreateSut().CreateToken(user);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(result.Token);
        var jti = jwt.Claims.First(c => c.Type == "jti").Value;

        Assert.Equal(result.TokenId, jti);
    }
}
