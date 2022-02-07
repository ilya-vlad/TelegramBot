using API.ApiRapid.Models;
using API.Common.Interfaces;
using API.Common.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

        private const string DateFormat = "yyyy-MM-dd";
        private const string QueryNameParam = "base";
        private const string BaseCurrency = "UAH";
        private const string HeaderHostNameParam = "x-rapidapi-host";
        private const string HeaderKeyNameParam = "x-rapidapi-key";

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

            var request = new RestRequest($"{date.ToString(DateFormat)}");
            
            request.AddQueryParameter(QueryNameParam, BaseCurrency);
            request.AddHeader(HeaderHostNameParam, _options.HeaderHost);
            request.AddHeader(HeaderKeyNameParam, _options.HeaderKey);

            var queryResult = MakeRequest(request);
            _logger.LogInformation($"Downloaded JSON from RapidApi. Url: {queryResult.ResponseUri}");

            if (queryResult.ResponseStatus == ResponseStatus.Completed)
            {
                try
                {
                    result = DeserializeJson(queryResult.Content);
                }
                catch (NullReferenceException ex)
                {
                    _logger.LogError($"Json is empty.\n{ex.Message}");
                }
                catch (FormatException ex)
                {
                    _logger.LogError($"Json is not in a valid format.\n{ex.Message}");
                }
                catch (JsonSerializationException ex )
                {
                    _logger.LogError($"Json integrity broken.\n{ex.Message}");
                }
                catch(JsonReaderException ex)
                {
                    _logger.LogError($"Failed to read JSON.\n{ex.Message}");
                }                
                catch (Exception ex)
                {
                    _logger.LogError($"{ex.Message}");
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
                (json, new IsoDateTimeConverter { DateTimeFormat = DateFormat });

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