using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace API.ApiPrivatBank.Models
{
    public class DailyExchangeRatesPrivatBank
    {
        public DateTime Date { get ; set; }

        public string Bank { get; set; }

        public string BaseCurrencyLit { get; set; }

        public int BaseCurrency { get; set; }


        [JsonProperty("ExchangeRate")]
        public List<ExchangeRatePrivatBank> ExchangeRates { get; set; }
    }
}
