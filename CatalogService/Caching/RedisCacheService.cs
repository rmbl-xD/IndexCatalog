using System.Text.Json;
using CatalogService.Models;
using StackExchange.Redis;

namespace CatalogService.Caching;

public class RedisCacheService : ICacheService 
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<string> GetCacheValueAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var redisValue = await db.StringGetAsync(key);
        return redisValue.HasValue ? redisValue.ToString() : string.Empty;
    }

    public async Task SetCacheValueAsync(string key, string value)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.StringSetAsync(key, value);
    }

    public async Task<T?> GetCacheValueAsync<T>(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var redisValue = await db.StringGetAsync(key);

        if (!redisValue.HasValue) return default;
        var value = JsonSerializer.Deserialize<T>(redisValue);
        return value;
    }

    public async Task SetCacheValueAsync<T>(string key, T value)
    {
        var db = _connectionMultiplexer.GetDatabase();

        var serialized = JsonSerializer.Serialize(value);
        await db.StringSetAsync(key, serialized);
    }

    public bool DeleteCacheValue(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        return db.KeyDelete(key);
    }
}