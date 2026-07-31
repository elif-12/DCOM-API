using DCOM_API.Application.Interfaces;
using StackExchange.Redis;

namespace DCOM_API.Infrastructure;

public class RedisStore : ITokenStore
{
    private readonly IDatabase _db;

    public RedisStore(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<bool> ExistsAsync(string tokenId) =>
        await _db.KeyExistsAsync(Key(tokenId));

    public async Task SetAsync(string tokenId, TimeSpan ttl) =>
        await _db.StringSetAsync(Key(tokenId), "1", ttl);

    public async Task RemoveAsync(string tokenId) =>
        await _db.KeyDeleteAsync(Key(tokenId));

    private static RedisKey Key(string tokenId) => $"token:{tokenId}";
}