using API.Common.Interfaces;
using API.Common.Models;
using CacheContext;
using System;
using System.Linq;

namespace Bot.Services.Strategies
{
    public class CorrectRequestStrategy : BaseResponseStrategy
    {
        public CorrectRequestStrategy(ICacheManager cache, ICurrencyDataProvider currencyProvider)
            : base(cache, currencyProvider)
        {
        }

        public override string GetResponse(string currency, DateTime date)
        {
            DailyExchangeRates exchangeRates = null;

            if (!_cache.Contains(date))
            {
                exchangeRates = _currencyProvider.GetExchangeRates(date);
                _cache.Add(exchangeRates);
            }

            exchangeRates ??= (DailyExchangeRates)_cache.GetByDate(date); 

            ExchangeRate rate = exchangeRates.ExchangeRates
                .Where(r => !string.IsNullOrEmpty(r.CurrencyName) && r.CurrencyName.ToLower() == currency.ToLower())
                .FirstOrDefault();

            string response = $"{Resources.Messages.ExchangeRateNotFound} {date.Date.ToShortDateString()}";

            if (rate != null)
            {
                response = $"{Resources.Messages.ExchangeRateOn} {date.Date.ToShortDateString()}\n" +
                $"1 {rate.CurrencyName} = {Math.Round(rate.Rate, 4)} {Resources.Messages.CurrencyUAH}";                
            }

            return response;
        }
    }
}