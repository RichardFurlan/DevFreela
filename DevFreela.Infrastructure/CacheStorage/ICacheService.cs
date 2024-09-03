namespace DevFreela.Infrastructure.CacheStorage;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key,CancellationToken cancellationToken = default)
        where T : class;
    Task SetAsync<T>(string key, T data, int secondsForExpiration = 3600, int secondsForExpirationSliding = 1200, CancellationToken cancellationToken = default)
        where T: class;
}