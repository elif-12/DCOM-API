using System.IdentityModel.Tokens.Jwt;
using DCOM_API.Application.Interfaces;

namespace DCOM_API.Middleware;

public class IdleTimeoutMiddleware
{
    private readonly RequestDelegate _next;
    private readonly int _idleMinutes;

    public IdleTimeoutMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _idleMinutes = config.GetValue<int>("Jwt:IdleMinutes");
    }

    // ITokenStore method injection ile geliyor
    public async Task InvokeAsync(HttpContext context, ITokenStore store)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var tokenId = context.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            if (tokenId is not null)
            {
                if (!await store.ExistsAsync(tokenId))
                    throw new UnauthorizedAccessException("Oturum zaman aşımına uğradı. Lütfen tekrar giriş yapın.");

                await store.SetAsync(tokenId, TimeSpan.FromMinutes(_idleMinutes));  // tazele
            }
        }

        await _next(context);
    }
}