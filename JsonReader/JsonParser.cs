using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using Common.Models;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JsonDeserializer
{
    public class JsonParser
    {
        private readonly ILogger<JsonParser> _logger;        
        private readonly IConfiguration _config;

        public JsonParser(ILogger<JsonParser> logger, IConfiguration config)
        {
            _logger = logger;           
            _config = config;
        }

        public DayExchangeRates GetExchangeRates(DateTime date)
        {           
            string json = DownloadJson(date);

            return string.IsNullOrEmpty(json) ? null : DeserializeJson(json);
        }

        private DayExchangeRates DeserializeJson(string json)
        {            
            DayExchangeRates myDeserializedClass = JsonConvert.DeserializeObject<DayExchangeRates>(json, 
                new IsoDateTimeConverter { DateTimeFormat = "dd.MM.yyyy"});

            return myDeserializedClass;
        }

        private string DownloadJson(DateTime date)
        {
            string apiUrl = _config.GetValue<string>("Api:PrivateBankUrl");

            if (string.IsNullOrEmpty(apiUrl))
            {
                throw new ArgumentNullException("Url api from appsettings is null.");
            }

            string paramUrl = _config.GetValue<string>("Api:UrlParameter");

            if (string.IsNullOrEmpty(paramUrl))
            {
                throw new ArgumentNullException("Url parameter from appsettings is null.");
            }

            try
            {
                var client = new RestClient(apiUrl);

                var request = new RestRequest(paramUrl, Method.Get);

                request.AddQueryParameter("date", date.ToString("dd.MM.yyyy"));

                var queryResult = client.ExecuteAsync(request).Result;

                if (queryResult.ResponseStatus == ResponseStatus.Completed)
                {
                    _logger.LogInformation($"Downloaded JSON from '{queryResult.ResponseUri}'");
                    return queryResult.Content;
                }

                return null;
            }
            catch(Exception e)
            {
                _logger.LogCritical($"{e.Message}");
                return null;
            }
        }
    }
}