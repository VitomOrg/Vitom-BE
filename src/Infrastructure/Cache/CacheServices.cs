using Application.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Infrastructure.Cache;

public class CacheServices(IDistributedCache distributedCache) : ICacheServices
{
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        string? cachedValue = await distributedCache.GetStringAsync(key, cancellationToken);
        if (cachedValue is null)
        {
            return null;
        }
        T? value = JsonConvert.DeserializeObject<T>(cachedValue);
        return value;
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);
        CacheKeys.TryRemove(key, out bool _);
    }

    public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {
        IEnumerable<Task> tasks = CacheKeys.Keys.Where(k => k.StartsWith(prefixKey)).Select(k => RemoveAsync(k, cancellationToken));
        await Task.WhenAll();
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        string cacheValue = JsonConvert.SerializeObject(value);
        await distributedCache.SetStringAsync(key, cacheValue, CacheOptions.DefaultExpiration, cancellationToken);
        CacheKeys.TryAdd(key, false);
    }
}