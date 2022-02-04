using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using API.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using API.ApiPrivatBank.Models;
using API.Common.Models;

namespace API.ApiPrivatBank
{
    public class CurrencyDataProviderPrivatBank : ICurrencyDataProvider
    {
        private readonly ILogger<CurrencyDataProviderPrivatBank> _logger;
        private readonly ApiPrivatBankOptions _options;
        private readonly IRestClient _client;

        public CurrencyDataProviderPrivatBank(
            ILogger<CurrencyDataProviderPrivatBank> logger, 
            ApiPrivatBankOptions options, 
            IRestClient client)
        {
            _logger = logger;
            _options = options;
            _client = client;
        }       

        public DailyExchangeRates GetExchangeRates(DateTime date)
        {
            DailyExchangeRates result = null;

            var request = new RestRequest();

            request.AddQueryParameter("date", date.ToString("dd.MM.yyyy"));

            var queryResult = MakeRequest(request);
            _logger.LogInformation($"Downloaded JSON from API PrivatBank. Url: {queryResult.ResponseUri}");

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
            DailyExchangeRatesPrivatBank exchangeRatesPrivatBank = 
                JsonConvert.DeserializeObject<DailyExchangeRatesPrivatBank>
                (json, new IsoDateTimeConverter { DateTimeFormat = "dd.MM.yyyy" });

            DailyExchangeRates exchangeRates = ConvertToDailyExchangeRates(exchangeRatesPrivatBank);

            return exchangeRates;
        }

        private DailyExchangeRates ConvertToDailyExchangeRates(DailyExchangeRatesPrivatBank exchangeRatesPrivatBank)
        {
            List<ExchangeRate> listCurrencies = exchangeRatesPrivatBank.ExchangeRates
                .Select(currency => new ExchangeRate() 
                { 
                    CurrencyName = currency.Currency, 
                    Rate = currency.SaleRateNB 
                })
                .ToList();

            DailyExchangeRates exchangeRates = new()
            {
                Date = exchangeRatesPrivatBank.Date,
                BaseCurrencyName = exchangeRatesPrivatBank.BaseCurrencyLit,
                ExchangeRates = listCurrencies
            };

            return exchangeRates;
        }
    }
}