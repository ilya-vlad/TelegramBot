using System;
using System.Collections.Generic;
using System.Linq;

namespace Bot.Common
{
    public class ExchangeRatesOfOneDay
    {
        public DateTime Date { get; set; }

        public List<CurrencyItem> Currencies { get; set; }

        public ExchangeRatesOfOneDay(DateTime day)
        {
            Date = day.Date;
            Currencies = new();
        }

        public CurrencyItem GetCurrency(string nameCurrency)
        {
            return Currencies.Where(c => c.NameCurrency.ToLower() == nameCurrency.ToLower()).FirstOrDefault();
        }
    }
}
