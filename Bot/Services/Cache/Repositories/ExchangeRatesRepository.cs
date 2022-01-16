using Bot.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;

namespace Bot.Services.Cache.Repositories
{
    public class ExchangeRatesRepository : BaseRepository<ExchangeRatesRepository>
    {
        public ExchangeRatesRepository(ILogger<ExchangeRatesRepository> logger, IMemoryCache cache) : base(logger, cache)
        {
        }

        public void Add(ExchangeRatesOfOneDay exchangeRates)
        {
            if (exchangeRates == null)
            {
                return;
            }

            if (!_cache.TryGetValue(exchangeRates.Date, out _))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1));
                _cache.Set(exchangeRates.Date, exchangeRates);

                _logger.LogInformation($"Added currencies on {exchangeRates.Date.ToShortDateString()} to cache.");
            }
        }

        public ExchangeRatesOfOneDay GetByDate(DateTime date)
        {
            var day = date.Date;
            ExchangeRatesOfOneDay exchangeRates;

            if (_cache.TryGetValue(day, out exchangeRates))
            {
                return exchangeRates;
            }

            return null;
        }

        public bool IsEmpty(DateTime date)
        {
            return !_cache.TryGetValue(date, out _);
        }
    }
}
