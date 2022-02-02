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

        public CurrencyDataProviderRapid(ILogger<CurrencyDataProviderRapid> logger, ApiRapidOptions options)
        {
            _logger = logger;
            _options = options;
        }

        public DailyExchangeRates GetExchangeRates(DateTime date)
        {            
            string json = DownloadJson(date);

            return string.IsNullOrEmpty(json) ? null : DeserializeJson(json);
        }

        private string DownloadJson(DateTime date)
        {
            try
            {
                string apiUrl = _options.Url;

                if (string.IsNullOrEmpty(apiUrl))
                {
                    throw new ArgumentNullException(nameof(apiUrl), "Url api is null.");
                }

                string headerHost = _options.HeaderHost;

                if (string.IsNullOrEmpty(headerHost))
                {
                    throw new ArgumentNullException(nameof(headerHost), "Header host is null.");
                }

                string headerKey = _options.HeaderKey;

                if (string.IsNullOrEmpty(headerKey))
                {
                    throw new ArgumentNullException(nameof(headerKey), "Header key is null.");
                }

                var client = new RestClient($"{apiUrl}{date:yyyy-MM-dd}");

                var request = new RestRequest();

                request.AddQueryParameter("base", "UAH");
                request.AddHeader("x-rapidapi-host", headerHost);
                request.AddHeader("x-rapidapi-key", headerKey);


                var queryResult = client.ExecuteAsync(request).Result;

                if (queryResult.ResponseStatus == ResponseStatus.Completed)
                {
                    _logger.LogInformation($"Downloaded JSON from RapiAPI. Url: {queryResult.ResponseUri}");
                    return queryResult.Content;
                }

                return null;
            }
            catch (Exception e)
            {
                _logger.LogCritical($"{e.Message}");
                return null;
            }
        }       

        private DailyExchangeRates DeserializeJson(string json)
        {
            DailyExchangeRatesRapid exchangeRatesRapid = JsonConvert.DeserializeObject<DailyExchangeRatesRapid>(json,
                new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" });

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