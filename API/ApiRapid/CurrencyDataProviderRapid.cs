using API.ApiRapid.Models;
using API.Common.Interfaces;
using API.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.ApiRapid
{
    public class CurrencyDataProviderRapid : ICurrencyDataProvider
    {
        private readonly ILogger<CurrencyDataProviderRapid> _logger;
        private readonly ApiRapidOptions _options;
        private readonly IRestClient _client;

        public CurrencyDataProviderRapid(
            ILogger<CurrencyDataProviderRapid> logger,
            ApiRapidOptions options, 
            IRestClient client)
        {
            _logger = logger;
            _options = options;
            _client = client;
        }

        public DailyExchangeRates GetExchangeRates(DateTime date)
        {
            DailyExchangeRates result = null;

            var request = new RestRequest($"{date:yyyy-MM-dd}");
            
            request.AddQueryParameter("base", "UAH");
            request.AddHeader("x-rapidapi-host", _options.HeaderHost);
            request.AddHeader("x-rapidapi-key", _options.HeaderKey);

            var queryResult = MakeRequest(request);
            _logger.LogInformation($"Downloaded JSON from RapidApi. Url: {queryResult.ResponseUri}");

            if (queryResult.ResponseStatus == ResponseStatus.Completed)
            {
                try
                {
                    result = DeserializeJson(queryResult.Content);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"{ex.Message}");
                    return result;
                }
            }

            return result;
        }

        private IRestResponse MakeRequest(RestRequest request)
        {
            _client.BaseUrl = new Uri(_options.Url);

            return _client.Execute(request);
        }

        private DailyExchangeRates DeserializeJson(string json)
        {
            DailyExchangeRatesRapid exchangeRatesRapid = 
                JsonConvert.DeserializeObject<DailyExchangeRatesRapid>
                (json, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" });

            DailyExchangeRates exchangeRates = ConvertToDailyExchangeRates(exchangeRatesRapid);

            return exchangeRates;
        }

        private DailyExchangeRates ConvertToDailyExchangeRates(DailyExchangeRatesRapid exchangeRatesRapid)
        {
            List<ExchangeRate> listCurrencies = exchangeRatesRapid.Rates
               .Select(currency => new ExchangeRate()
               {
                   CurrencyName = currency.Key,
                   Rate = 1 / currency.Value
               })
               .ToList();

            DailyExchangeRates exchangeRates = new()
            {
                Date = exchangeRatesRapid.Date,
                BaseCurrencyName = exchangeRatesRapid.BaseCurrency,
                ExchangeRates = listCurrencies
            };

            return exchangeRates;
        }
    }
}