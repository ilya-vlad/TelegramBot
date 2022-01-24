using CacheContext;
using Common.Models;
using JsonDeserializer;
using System;
using System.Linq;

namespace Bot.Services.Strategies
{
    public class CorrectRequestStrategy : BaseResponseStrategy<CorrectRequestStrategy>
    {
        public CorrectRequestStrategy(CacheManager cache, JsonParser parser)
            : base(cache, parser)
        {
        }

        public override string GetResponse(string currency, DateTime date)
        {
            DayExchangeRates exchangeRates;

            if (!_cache.Contains(date))
            {
                exchangeRates = _parser.GetExchangeRates(date);                
                _cache.Add(exchangeRates);
            }

            exchangeRates = _cache.GetByDate(date);
            
            ExchangeRate rate = exchangeRates.ExchangeRate
                .Where(r => !string.IsNullOrEmpty(r.Currency) && r.Currency.ToLower() == currency.ToLower())
                .FirstOrDefault();

            string response = string.Empty;

            if(rate != null)
            {
                response = $"{Resources.Messages.ExchangeRateOn} {date.Date.ToShortDateString()}\n" +
                $"1 {rate.Currency} = {rate.SaleRateNB} {Resources.Messages.CurrencyUAH}";                
            }
            else
            {
                response = $"{Resources.Messages.ExchangeRateNotFound} {date.Date.ToShortDateString()}";
            }

            return response;
        }
    }
}