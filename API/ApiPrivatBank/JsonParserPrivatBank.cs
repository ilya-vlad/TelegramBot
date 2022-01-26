using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Api.ApiPrivatBank.Models;
using API.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace API.ApiPrivatBank
{
    public class JsonParserPrivatBank : IJsonParser
    {
        private readonly ILogger<JsonParserPrivatBank> _logger;
        private readonly IConfiguration _config;

        public JsonParserPrivatBank(ILogger<JsonParserPrivatBank> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
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
                string apiUrl = _config.GetValue<string>("Api:PrivatBank:Url");

                if (string.IsNullOrEmpty(apiUrl))
                {
                    throw new ArgumentNullException(nameof(apiUrl), "Url api is null.");
                }               

                var client = new RestClient(apiUrl);
                
                var request = new RestRequest();

                request.AddQueryParameter("date", date.ToString("dd.MM.yyyy"));

                var queryResult = client.ExecuteAsync(request).Result;

                if (queryResult.ResponseStatus == ResponseStatus.Completed)
                {
                    _logger.LogInformation($"Downloaded JSON from API PrivatBank. Url: {queryResult.ResponseUri}");
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
            DailyExchangeRatesPrivateBank exchangeRatesPrivateBank = JsonConvert.DeserializeObject<DailyExchangeRatesPrivateBank>(json,
                new IsoDateTimeConverter { DateTimeFormat = "dd.MM.yyyy" });

            DailyExchangeRates exchangeRates = ConvertToDailyExchangeRates(exchangeRatesPrivateBank);

            return exchangeRates;
        }

        private DailyExchangeRates ConvertToDailyExchangeRates(DailyExchangeRatesPrivateBank exchangeRatesPrivateBank)
        {
            List<ExchangeRate> listCurrencies = exchangeRatesPrivateBank.ExchangeRates
                .Select(currency => new ExchangeRate() 
                { 
                    CurrencyName = currency.Currency, 
                    Rate = currency.SaleRateNB 
                })
                .ToList();

            DailyExchangeRates exchangeRates = new()
            {
                Date = exchangeRatesPrivateBank.Date,
                BaseCurrencyName = exchangeRatesPrivateBank.BaseCurrencyLit,
                ExchangeRates = listCurrencies
            };

            return exchangeRates;
        }
    }
}