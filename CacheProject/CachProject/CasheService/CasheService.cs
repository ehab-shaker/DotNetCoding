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

        public CacheService(IMemoryCache iMemoryCache, IOptions<CacheSettings> cacheSettings)
        {
            _IMemoryCache = iMemoryCache;
            _cacheSettings = cacheSettings.Value;
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<string, Task<T>> func, string parameter)
        {
            var cacheExpiryOptions = GetCachSettings();
            if (_cacheSettings.Enabled)
            {
                if (_IMemoryCache.TryGetValue(key, out T cachedData))
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
                            var freashData = await func(parameter);
                            _IMemoryCache.Set(key, freashData, cacheExpiryOptions);
                            return freashData;
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
        public async Task<T> GetOrCreateAsync<T>(string key, Func<int, Task<T>> func, int parameter)
        {
            var cacheExpiryOptions = GetCachSettings();
            if (_cacheSettings.Enabled)
            {
                if (_IMemoryCache.TryGetValue(key, out T cachedData))
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
                            var freashData = await func(parameter);
                            _IMemoryCache.Set(key, freashData, cacheExpiryOptions);
                            return freashData;
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
        public async Task<T> GetOrCreateWithGenericAsync<T, TParameter>(string key, Func<TParameter, Task<T>> func, TParameter parameter)
        {
            var cacheExpiryOptions = GetCachSettings();
            if (_cacheSettings.Enabled)
            {
                if (_IMemoryCache.TryGetValue(key, out T cachedData))
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
                            var freashData = await func(parameter);
                            _IMemoryCache.Set(key, freashData, cacheExpiryOptions);
                            return freashData;
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
        public object GetMemoryValueByKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            try
            {
                _IMemoryCache.TryGetValue(key, out object cachedData);
                return cachedData;
            }
            catch
            {
                return null;
            }
        }
        private MemoryCacheEntryOptions GetCachSettings()
        {
            if (_cacheSettings == null)
            {
                return null;
            }
            else
            {
                return new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(_cacheSettings.AbsoluteExpiration),
                    SlidingExpiration = TimeSpan.FromSeconds(_cacheSettings.SlidingExpiration),
                    Priority = CacheItemPriority.Normal,
                };
            }
        }
    }
}
