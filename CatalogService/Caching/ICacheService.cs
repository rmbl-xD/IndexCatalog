namespace CatalogService.Caching;

public interface ICacheService
{
    Task<string> GetCacheValueAsync(string key);
    Task SetCacheValueAsync(string key, string value);
    
    Task<T?> GetCacheValueAsync<T>(string key);
    Task SetCacheValueAsync<T>(string key, T value);

    bool DeleteCacheValue(string key);
}