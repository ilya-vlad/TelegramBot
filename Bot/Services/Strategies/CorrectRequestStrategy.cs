using API.Common.Interfaces;
using API.Common.Models;
using CacheContext;
using System;
using System.Linq;

namespace Bot.Services.Strategies
{
    public class CorrectRequestStrategy : BaseResponseStrategy<CorrectRequestStrategy>
    {
        public CorrectRequestStrategy(CacheManager cache, ICurrencyDataProvider parser)
            : base(cache, parser)
        {
        }

        public override string GetResponse(string currency, DateTime date)
        {
            DailyExchangeRates exchangeRates;

            if (!_cache.Contains(date))
            {
                exchangeRates = _parser.GetExchangeRates(date);
                _cache.Add(exchangeRates);
            }
            else
            {
                exchangeRates = _cache.GetByDate(date);
            }

            ExchangeRate rate = exchangeRates.ExchangeRates
                .Where(r => !string.IsNullOrEmpty(r.CurrencyName) && r.CurrencyName.ToLower() == currency.ToLower())
                .FirstOrDefault();

            string response = string.Empty;

            if(rate != null)
            {
                response = $"{Resources.Messages.ExchangeRateOn} {date.Date.ToShortDateString()}\n" +
                $"1 {rate.CurrencyName} = {Math.Round(rate.Rate, 4)} {Resources.Messages.CurrencyUAH}";                
            }
            else
            {
                response = $"{Resources.Messages.ExchangeRateNotFound} {date.Date.ToShortDateString()}";
            }

            return response;
        }
    }
}