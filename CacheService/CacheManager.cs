using API.Common.Models;
using CacheContext.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;

namespace CacheContext
{
    public class CacheManager
    {
        private readonly ILogger<CacheManager> _logger;
        private readonly IMemoryCache _cache;
        private readonly CacheManagerOptions _options;

        private MemoryCacheEntryOptions options;

        public CacheManager(ILogger<CacheManager> logger, IMemoryCache cache, CacheManagerOptions options)
        {
            _logger = logger;
            _cache = cache;
            _options = options;

            SetCacheOptions();
        }

        private void SetCacheOptions()
        {
            double? minutes = _options.StorageTimeInMin;

            minutes = minutes == null ? 60 : minutes;

            options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes.Value));
        }

        public void Add(DailyExchangeRates exchangeRates)
        {
            if(exchangeRates == null)
            {
                return;
            }

            if(!_cache.TryGetValue(exchangeRates.Date, out _))
            {
                _cache.Set(exchangeRates.Date.Date, exchangeRates, options);
                _logger.LogInformation($"Added currencies on {exchangeRates.Date.ToShortDateString()} to cache.");
            }
        }

        public DailyExchangeRates GetByDate(DateTime date)
        {
            DailyExchangeRates exchangeRates;

            if (_cache.TryGetValue(date.Date.Date, out exchangeRates))
            {
                return exchangeRates;
            }

            return null;
        }

        public bool Contains(DateTime date) => _cache.TryGetValue(date.Date, out _);
    }
}