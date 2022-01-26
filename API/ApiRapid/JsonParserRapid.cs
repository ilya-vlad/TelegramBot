using Api.ApiPrivatBank.Models;
using API.ApiRapid.Models;
using API.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.ApiRapid
{
    public class JsonParserRapid : IJsonParser
    {
        private readonly ILogger<JsonParserRapid> _logger;
        private readonly IConfiguration _config;

        public JsonParserRapid(ILogger<JsonParserRapid> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public DailyExchangeRates GetExchangeRates(DateTime date)
        {
            string incorrectJson = DownloadJson(date);     
            
            if(string.IsNullOrEmpty(incorrectJson))
            {
                return null;
            }            

            string correctJson = GetCorrectJson(incorrectJson);

            DailyExchangeRates exchangeRates = DeserializeJson(correctJson);

            return exchangeRates;
        }

        private string DownloadJson(DateTime date)
        {
            try
            {
                string apiUrl = _config.GetValue<string>("Api:RapidApi:Url");

                if (string.IsNullOrEmpty(apiUrl))
                {
                    throw new ArgumentNullException(nameof(apiUrl), "Url api is null.");
                }

                string headerHost = _config.GetValue<string>("Api:RapidApi:HeadersParameter:x-rapidapi-host");

                if (string.IsNullOrEmpty(headerHost))
                {
                    throw new ArgumentNullException(nameof(headerHost), "Header host is null.");
                }

                string headerKey = _config.GetValue<string>("Api:RapidApi:HeadersParameter:x-rapidapi-key");

                if (string.IsNullOrEmpty(headerKey))
                {
                    throw new ArgumentNullException(nameof(headerKey), "Header key is null.");
                }

                var client = new RestClient($"{apiUrl}{date.ToString("yyyy-MM-dd")}");

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

        private string GetCorrectJson(string incorrectJson)
        {           
            JObject jsonObject = JObject.Parse(incorrectJson);
            JObject jsonRates = (JObject)jsonObject["rates"];

            jsonObject.Remove("rates");
            JArray newRates = new JArray();

            foreach (JProperty x in (JToken)jsonRates)
            {               
                JObject objectCurrency = new JObject();
                objectCurrency["currency"] = x.Name;
                objectCurrency["rate"] = x.Value;
                newRates.Add(objectCurrency);
            }

            jsonObject.Add("rates", newRates);

            var correctJson = JsonConvert.SerializeObject(jsonObject);
            
            return correctJson;
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
                   CurrencyName = currency.CurrencyName,
                   Rate = 1 / currency.Rate
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
