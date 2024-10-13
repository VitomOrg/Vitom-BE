using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Cache;

public class CacheOptions
{
    public static DistributedCacheEntryOptions DefaultExpiration =>
        new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) };
}