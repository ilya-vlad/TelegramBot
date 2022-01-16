using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using Newtonsoft.Json.Linq;
using Bot.Common;
using Bot.Services.Cache;

namespace Bot.Services
{
    public class JsonParser
    {
        private readonly ILogger<JsonParser> _logger;
        private readonly CacheService _cache;
        private readonly IConfiguration _config;

        public JsonParser(ILogger<JsonParser> logger, CacheService cache, IConfiguration config)
        {
            _logger = logger;
            _cache = cache;
            _config = config;
        }

        public void ParseToCache(DateTime date)
        {
            string json = DownloadJson(date);

            ExchangeRatesOfOneDay exchangeRates = ParseJson(json, date);

            _cache.ExchangeRates.Add(exchangeRates);
        }

        private ExchangeRatesOfOneDay ParseJson(string json, DateTime date)
        {
            JObject jsonObject = JObject.Parse(json);
            JArray jsonArray = (JArray)jsonObject["exchangeRate"];

            var newExchangeRates= new ExchangeRatesOfOneDay(date);

            foreach(JObject currency in jsonArray)
            {
                if (string.IsNullOrEmpty((string)currency["currency"]))                
                    continue;
                
                var newCurrencyItem = new CurrencyItem()
                {
                    NameCurrency = (string)currency["currency"],
                    ValueCurrency = (float)currency["saleRateNB"]
                };

                newExchangeRates.Currencies.Add(newCurrencyItem);
            }

            return newExchangeRates;            
        }

        private string DownloadJson(DateTime date)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var uri = new Uri(_config.GetValue<string>("Api:PrivateBankCurrencies") + date.ToString("dd.MM.yyyy"));
                    string json = wc.DownloadString(uri);
                    _logger.LogInformation($"Downloaded JSON from '{uri}'");

                    return json;
                }
            }
            catch(Exception e)
            {
                _logger.LogCritical($"{e.Message}\nCheck url apibank.");
                return string.Empty;
            }
        }
    }
}