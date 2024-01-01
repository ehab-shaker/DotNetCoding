using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
#nullable disable

namespace CachProject.CashService
{
    public class CacheService : ICache
    {
        private readonly IMemoryCache _IMemoryCache;
        private readonly CacheSettings _cacheSettings;
        private readonly SemaphoreSlim _semaphore;
        private ILogger<CacheService> _logger;

        public CacheService(IMemoryCache iMemoryCache, IOptions<CacheSettings> cacheSettings, ILogger<CacheService> logger)
        {
            _IMemoryCache = iMemoryCache;
            _cacheSettings = cacheSettings.Value;
            _semaphore = new SemaphoreSlim(1, 1);
            _logger = logger;
        }

        public async Task<TResult> GetOrCreateAsync<TResult, TParameter>(string key, Func<TParameter, Task<TResult>> func, TParameter parameter)
        {
            var cacheExpiryOptions = GetCachSettings();
            if (_cacheSettings.Enabled)
            {
                if (_IMemoryCache.TryGetValue(key, out TResult cachedData))
                {
                    return cachedData;
                }
                else
                {
                    try
                    {
                        await _semaphore.WaitAsync();
                        if (_IMemoryCache.TryGetValue(key, out cachedData))
                        {
                            return cachedData;
                        }
                        else
                        {
                            var freshData = await func(parameter);
                            _IMemoryCache.Set(key, freshData, cacheExpiryOptions);
                            return freshData;
                        }
                    }
                    catch
                    {
                        return await func(parameter);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
            }
            else
            {
                return await func(parameter);
            }
        }
        public async Task<TResult> GetOrCreateAsync_UsingBuiltInFunction<TResult, TParameter>(string key, Func<TParameter, Task<TResult>> func, TParameter parameter)
        {
            var cacheExpiryOptions = GetCachSettings();
            return await _IMemoryCache.GetOrCreateAsync(key, async cacheEntry =>
            {
                GetCachSettings();
                return await func(parameter);
            });
        }


        private MemoryCacheEntryOptions GetCachSettings()
        {
            return _cacheSettings == null ?
            new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(400),
                SlidingExpiration = TimeSpan.FromSeconds(200),
                Priority = CacheItemPriority.Normal,
            } :
            new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(_cacheSettings.AbsoluteExpiration),
                SlidingExpiration = TimeSpan.FromSeconds(_cacheSettings.SlidingExpiration),
                Priority = CacheItemPriority.Normal,
            };
        }
    }
}
