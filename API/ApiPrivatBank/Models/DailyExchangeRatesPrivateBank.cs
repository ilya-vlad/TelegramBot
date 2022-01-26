using API.Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Api.ApiPrivatBank.Models
{
    public class DailyExchangeRatesPrivateBank
    {
        public DateTime Date { get ; set; }

        public string Bank { get; set; }

        public string BaseCurrencyLit { get; set; }

        public int BaseCurrency { get; set; }


        [JsonProperty("ExchangeRate")]
        public List<ExchangeRatePrivateBank> ExchangeRates { get; set; }
    }
}
