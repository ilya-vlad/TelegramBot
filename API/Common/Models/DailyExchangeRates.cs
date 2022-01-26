using System;
using System.Collections.Generic;

namespace Api.ApiPrivatBank.Models
{
    public class DailyExchangeRates
    {
        public DateTime Date { get ; set; }        

        public string BaseCurrencyName { get; set; }        
        
        public List<ExchangeRate> ExchangeRates { get; set; }
    }
}
