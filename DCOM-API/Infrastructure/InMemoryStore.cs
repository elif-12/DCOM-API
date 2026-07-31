using DCOM_API.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace DCOM_API.Infrastructure;

public class InMemoryStore : ITokenStore
{
    private readonly IMemoryCache _cache;

    public InMemoryStore(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<bool> ExistsAsync(string tokenId) =>
        Task.FromResult(_cache.TryGetValue(Key(tokenId), out _));

    public Task SetAsync(string tokenId, TimeSpan ttl)
    {
        _cache.Set(Key(tokenId), true, ttl);   // ttl kadar sonra otomatik silinir
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string tokenId)
    {
        _cache.Remove(Key(tokenId));
        return Task.CompletedTask;
    }

    private static string Key(string tokenId) => $"token:{tokenId}";
}