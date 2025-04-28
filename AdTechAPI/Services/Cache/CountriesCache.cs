using System.Text.Json;
using AdTechAPI.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace AdTechAPI.CachedData
{
    public class CountriesCache(
        RedisService redis,
        AppDbContext db,
        ILogger<CountriesCache> logger)
    {
        private readonly IDatabase _redis = redis.Db ?? throw new ArgumentNullException(nameof(redis));
        private readonly AppDbContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly ILogger<CountriesCache> _logger = logger ?? throw new ArgumentNullException(nameof(logger));


        private readonly TimeSpan _lockTimeout = TimeSpan.FromSeconds(5);
        private readonly TimeSpan _retryDelay = TimeSpan.FromMilliseconds(300);


        public async Task<int?> GetCountryIdByIso(string iso)
        {
            if (string.IsNullOrEmpty(iso))
            {
                _logger.LogWarning("GetCountryIdByIso called with null or empty ISO code");
                return null;
            }

            // Retry pattern for resilience using non-recursive approach
            for (int retryCount = 0; retryCount <= 1; retryCount++)
            {
                try
                {
                    var dictionary = await GetCountriesFromCache();
                    // optimisitic get
                    if (dictionary != null)
                    {
                        if (dictionary.TryGetValue(iso, out var countryId))
                        {
                            return countryId;
                        }

                        _logger.LogWarning("CountriesCache: ISO '{Iso}' not found in cache", iso);
                        return null;
                    }

                    // Cache miss or corruption detected - attempt rebuild
                    _logger.LogWarning("CountriesCache: Cache miss or corrupted. Attempt {RetryCount}", retryCount + 1);

                    // Only try to rebuild on first attempt
                    if (retryCount == 0)
                    {
                        await RebuildCacheWithLock();
                    }
                    else // someone else triggered
                    {
                        // On subsequent attempts, just go directly to DB
                        return await GetCountryIdFromDatabase(iso);
                    }
                }
                catch (RedisConnectionException redisEx)
                {
                    _logger.LogError(redisEx, "CountriesCache: Redis connection error. Falling back to database");
                    return await GetCountryIdFromDatabase(iso);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CountriesCache: Error while accessing country data. Attempt {RetryCount}", retryCount + 1);

                    if (retryCount > 0)
                    {
                        // Last attempt failed, fall back to database
                        return await GetCountryIdFromDatabase(iso);
                    }

                    // Add short delay before retry
                    await Task.Delay(_retryDelay);
                }
            }

            // Should not reach here but added for safety
            _logger.LogError("CountriesCache: All attempts failed for ISO {Iso}", iso);
            return 0;
        }

        public async Task BuildCountriesCache()
        {
            var countries = await _db.Countries
                .Select(c => new { c.Id, c.Iso })
                .ToDictionaryAsync(c => c.Iso, c => c.Id);

            var serialized = JsonSerializer.Serialize(countries);
            await _redis.StringSetAsync(CountriesCacheKeys.Key, serialized);
        }

        private async Task<Dictionary<string, int>?> GetCountriesFromCache()
        {
            var cachedData = await _redis.StringGetAsync(CountriesCacheKeys.Key);

            if (cachedData.IsNullOrEmpty)
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, int>>(cachedData.ToString());
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "CountriesCache: Failed to deserialize countries cache - cache corrupted");
                return null;
            }
        }

        private async Task<int?> GetCountryIdFromDatabase(string iso)
        {
            _logger.LogInformation("CountriesCache: Fetching country {Iso} directly from database", iso);
            try
            {
                var country = await _db.Countries.FirstOrDefaultAsync(c => c.Iso == iso);
                return country?.Id ?? null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CountriesCache: Database query failed for ISO {Iso}", iso);
                return 0;
            }
        }

        private async Task RebuildCacheWithLock()
        {
            var lockKey = CountriesCacheKeys.LockKey;
            var lockToken = Guid.NewGuid().ToString();

            // Set/Get redis lock key
            var lockAcquired = await _redis.StringSetAsync(
                lockKey,
                lockToken,
                _lockTimeout,
                When.NotExists); // IMPORTANT TO SET ONLY IF NOT EXIST

            if (lockAcquired)
            {
                _logger.LogInformation("CountriesCache: Lock acquired, rebuilding cache");
                try
                {
                    // Double check if another process already rebuilt the cache
                    var currentCache = await _redis.StringGetAsync(CountriesCacheKeys.Key);
                    if (currentCache.IsNullOrEmpty)
                    {
                        await BuildCountriesCache();
                    }
                    else
                    {
                        _logger.LogInformation("CountriesCache: Cache already rebuilt by another process");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CountriesCache: Error while rebuilding cache under lock");
                }
                finally
                {
                    // Release the lock only if we still own it
                    var currentLockValue = await _redis.StringGetAsync(lockKey);
                    if (currentLockValue == lockToken)
                    {
                        await _redis.KeyDeleteAsync(lockKey);
                        _logger.LogDebug("CountriesCache: Lock released");
                    }
                }
            }
            else
            {
                _logger.LogInformation("CountriesCache: Another process is rebuilding the cache, waiting");
                // Wait for other process to complete
                await Task.Delay(_retryDelay);
            }
        }

    }

    public static class CountriesCacheKeys
    {
        public const string Key = "cache::countries";
        public const string LockKey = "lock::countries_cache_rebuild";
    }

}
