using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace DevFreela.Infrastructure.CacheStorage;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        var cacheValue = await _cache.GetStringAsync(key, cancellationToken);

        return string.IsNullOrWhiteSpace(cacheValue) ? default(T) : JsonConvert.DeserializeObject<T>(cacheValue);
    }


    public async Task SetAsync<T>(string key, T data, int secondsForExpiration = 3600, int secondsForExpirationSliding = 1200, CancellationToken cancellationToken = default) where T : class
    {
        var memoryCacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(secondsForExpiration),
            SlidingExpiration = TimeSpan.FromSeconds(secondsForExpirationSliding)
        };
        
        var cacheValue = JsonConvert.SerializeObject(data);

        await _cache.SetStringAsync(key, cacheValue, memoryCacheEntryOptions, cancellationToken);
    }
}