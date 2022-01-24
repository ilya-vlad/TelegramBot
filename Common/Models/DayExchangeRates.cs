using System;
using System.Collections.Generic;


namespace Common.Models
{
    public class DayExchangeRates
    {
        public DateTime Date { get; set; }

        public string Bank { get; set; }

        public int BaseCurrency { get; set; }

        public string BaseCurrencyLit { get; set; }

        public List<ExchangeRate> ExchangeRate { get; set; }
    }
}
