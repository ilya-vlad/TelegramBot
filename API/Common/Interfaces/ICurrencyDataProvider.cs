using API.Common.Models;
using System;


namespace API.Common.Interfaces
{
    public interface ICurrencyDataProvider
    {
        public DailyExchangeRates GetExchangeRates(DateTime date);
    }
}
